import {Component, inject, input, OnInit, output, signal} from '@angular/core';
import {
  AbstractControl, FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule, ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import {RegisterCreds, User} from '../../../types/user';
import AccountService from '../../../core/services/account-service';
import {response} from 'express';
import {JsonPipe} from '@angular/common';
import {ValidationError} from '@angular/forms/signals';
import {TextInput} from '../../../shared/text-input/text-input';
import {Router} from '@angular/router';
import {sign} from 'node:crypto';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TextInput],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

  private accountService = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;
  protected credentialsForm: FormGroup;
  protected profileForm: FormGroup;
  protected currentSept = signal(1);
  protected validationErrors = signal<string[]>([]);

  constructor() {
    this.credentialsForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      displayName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    });
    this.credentialsForm.controls['password'].valueChanges.subscribe(() => {
      this.credentialsForm.controls['confirmPassword'].updateValueAndValidity();
    });

    this.profileForm = this.fb.group({
      gender: ['male', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
    })

  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const parent = control.parent;
      if (!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value === matchValue ? null : {passwordMismatch: true}
    }
  }

  nextStep() {
    if (this.credentialsForm.valid) {
      this.currentSept.update(prevStep => prevStep + 1);
    }
  }

  prevStep() {
    this.currentSept.update(prevStep => prevStep - 1);
  }

  getMaxDate() {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);
    return today.toISOString().split('T')[0];
  }

  register() {
    if (this.profileForm.valid && this.credentialsForm.valid) {
      const formData = {...this.credentialsForm.value, ...this.profileForm.value};
      this.accountService.register(formData).subscribe({
        next: response => {
          this.router.navigateByUrl('/members');
        },
        error: error => {
          console.log(error);
          this.validationErrors.set(error);
        }
      })
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
