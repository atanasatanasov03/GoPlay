import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-register-center',
  templateUrl: './register-center.component.html',
  styleUrls: ['./register-center.component.css']
})
export class RegisterCenterComponent implements OnInit {
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  logged: any;

  constructor(private userService: UserServiceService,
    private router: Router) {
    this.logged = userService.isLogged;
  }

  ngOnInit(): void {
  }

  register() {
    this.model.role = "center";
    console.log(this.model);
    this.userService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
      this.router.navigate(['/home'])
    }, error => {
      console.log(error);
    })
  }

  cancel() {
    console.log("canceled");
    this.router.navigate([''])
  }
}
