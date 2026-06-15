import {Component, inject, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {response} from 'express';
import {catchError} from 'rxjs';
import {environment} from '../../environments/environment';
import {Environment} from '@angular/cli/lib/config/workspace-schema';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.html',
  styleUrl: './test-errors.css',
})
class TestErrors {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  validationErrors = signal<string[]>([]);

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(
      {
        next: response => console.log(response),
        error: error => console.log(error)
      }
    )
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(
      {
        next: response => console.log(response),
        error: error => console.log(error)
      }
    )
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(
      {
        next: response => console.log(response),
        error: error => console.log(error)
      }
    )
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe(
      {
        next: response => console.log(response),
        error: error => console.log(error)
      }
    )
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe(
      {
        next: response => console.log(response),
        error: error => {
          console.log(error)
          this.validationErrors.set(error);

        }
      }
    )
  }

}

export default TestErrors
