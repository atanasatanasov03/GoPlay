import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  reactiveForm: FormGroup;
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  logged: any;

  constructor(private userService: UserServiceService,
    private notificationService: NotificationService,
    private router: Router,
    private builder: FormBuilder) {
    this.logged = userService.isLogged;
  }

  ngOnInit(): void {
    this.reactiveForm = this.builder.group({
      email: [null, Validators.required]
    });
  }

  register() {
    this.model.role = "regular";
    console.log(this.model);
    this.userService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
      this.notificationService.showSuccess("You have successfully registered your account", "")
      this.router.navigate(['/home'])
    }, error => {
      console.log(error);
    })
  }

  cancel() {
    console.log("canceled");
    this.cancelRegister.emit(false);
  }

}
