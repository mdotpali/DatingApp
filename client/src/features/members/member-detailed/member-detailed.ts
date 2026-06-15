import {Component, computed, inject, OnInit, signal} from '@angular/core';
import {MemberService} from '../../../core/services/member-service';
import {ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {filter} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {AgePipe} from '../../../core/pipes/age-pipe';
import AccountService from '../../../core/services/account-service';
import {PresenceService} from '../../../core/services/presence-service';
import {LikesService} from '../../../core/services/likes-service';


@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css',
})

export class MemberDetailed implements OnInit {

  protected memberService = inject(MemberService);
  private accountService = inject(AccountService);
  protected presenceService = inject(PresenceService);
  private route = inject(ActivatedRoute);
  private router = inject(Router)
  protected likeService= inject(LikesService);
  protected title= signal<string | undefined>('Profile');
  private routId = signal<string | null>(null);
  protected hasLiked = computed(() => this.likeService.likeIds().includes(this.routId()!));
  protected isCurrentUser = computed(() => {
    return this.accountService.currentUser()?.id === this.routId();
  });

  constructor() {
  this.route.paramMap.subscribe(paramMap => {
    this.routId.set(paramMap.get('id'));
  })
  }
  ngOnInit(): void {

   this.title.set(this.route.firstChild?.snapshot?.title);


    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title);
      }
    })
  }

  protected readonly AsyncPipe = AsyncPipe;
}
