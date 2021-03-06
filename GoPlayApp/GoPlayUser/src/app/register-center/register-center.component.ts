import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmPasswordValidator } from 'app/helpers/repeat-password.validator';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NotificationService } from '../services/notification.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-register-center',
  templateUrl: './register-center.component.html',
  styleUrls: ['./register-center.component.css']
})
export class RegisterCenterComponent implements OnInit {
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();

  sports: any;
  selectedItems: any;
  dropdownSettings: IDropdownSettings = {};

  registerForm: FormGroup;
  formSubmitted = false;
  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";

  constructor(private userService: UserServiceService,
    private notificationService: NotificationService,
    private router: Router,
    private builder: FormBuilder) { }

  ngOnInit(): void {
    this.registerForm = this.builder.group({
      email: ['', Validators.compose([Validators.pattern(this.emailPattern), Validators.required])],
      username: ['', Validators.compose([Validators.minLength(6), Validators.required])],
      sports: ['', Validators.required],
      address: ['', Validators.compose([Validators.minLength(10), Validators.required])],
      password: ['', Validators.compose([Validators.minLength(8), Validators.required])],
      repeatPassword: ['', Validators.required]
    },
    {
      validators: ConfirmPasswordValidator("password", "repeatPassword")
    })

    this.sports = [
      {item_id: 1, item_text: "Football"},
      {item_id: 2, item_text: "Basketball"},
      {item_id: 3, item_text: "Volleyball"},
      {item_id: 4, item_text: "Handball"},
      {item_id: 5, item_text: "Golf"},
      {item_id: 6, item_text: "Tennis"},
      {item_id: 7, item_text: "Ping Pong"},
      {item_id: 8, item_text: "Swimming"},
      {item_id: 9, item_text: "Track and Field"},
      {item_id: 10, item_text: "Billiards"}
    ];
    this.dropdownSettings = {
      singleSelection: false,
      defaultOpen: false,
      idField: 'item_id',
      textField: 'item_text',
      enableCheckAll: false,
      itemsShowLimit: 3,
      allowSearchFilter: true,
      limitSelection: 4
    };
  }

  register() {
    this.formSubmitted = true;

    if(this.registerForm.valid) {
      this.userService.register(this.buildModel()).subscribe(response => {
        console.log(response);
        this.cancel();
        this.notificationService.showSuccess("You have successfully registered your sports center", "")
        this.router.navigate(['/home'])
      }, error => {
        console.log(error);
      })
    }
  }

  buildModel(): any {
    return {
      userName: this.registerForm.get("username").value,
      role: "center",
      email: this.registerForm.get("email").value,
      password: this.registerForm.get("password").value,
      address: this.registerForm.get("address").value,
      sports: this.buildSports(this.registerForm.get("sports").value)
    }
  }

  cancel() {
    console.log(this.registerForm.get("email"))
    console.log("canceled");
    this.router.navigate([''])
  }

  buildSports(sports: any): string {
    var final = "";
    for(let i = 0; i < sports.length; i++) {
      final = final + sports[i].item_text + ';'
    }
    return final;
  }
}
