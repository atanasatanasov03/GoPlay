import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalStorageService } from 'app/services/local-storage.service';
import { UserServiceService } from 'app/services/user.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  id: string;

  constructor(private activatedRoute: ActivatedRoute,
    private userService: UserServiceService,
    private localStorage: LocalStorageService,
    private router: Router) {
    this.activatedRoute.queryParams.subscribe(params => {
      console.log("console log param: " + params.userId)
      this.id = params.userId
    });
  }

  ngOnInit(): void {
    this.userService.confirmEmail(this.id);
    this.router.navigate([''])
    this.localStorage.setUser(null);
  }

}
