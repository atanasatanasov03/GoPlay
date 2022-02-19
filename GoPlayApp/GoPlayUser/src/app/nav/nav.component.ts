import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from '../services/local-storage.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  logged: boolean;

  constructor(
    public accountService: UserServiceService,
    public localStorage: LocalStorageService,
    private router: Router
    ) { }

  ngOnInit(): void {
  }

  login() {
    console.log(this.model)
    this.accountService.login(this.model).subscribe(response => {
      this.localStorage.setToken(response)
      this.logged = this.accountService.isLogged();
      console.log("in login()", this.logged)

      this.router.navigate(['/home'])
    }, error => {
      console.log(error);
    })
  }

}
