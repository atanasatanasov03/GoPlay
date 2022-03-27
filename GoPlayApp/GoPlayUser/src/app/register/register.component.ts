import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NotificationService } from '../services/notification.service';
import { UserServiceService } from '../services/user.service';
import { ConfirmPasswordValidator } from '../helpers/repeat-password.validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
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
      firstName: ['', Validators.compose([Validators.minLength(2), Validators.required])],
      lastName: ['', Validators.compose([Validators.minLength(2), Validators.required])],
      age: ['', Validators.compose([Validators.min(14), Validators.max(60), Validators.required])],
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
        this.cancel();
        this.notificationService.showSuccess("You have successfully registered your account", "")
        this.router.navigate(['/home'])
      }, error => {
        console.log(error);
      })
    }
  }

  cancel() {
    console.log("canceled");
    this.cancelRegister.emit(false);
  }

  buildModel(): any {
    return {
      userName: this.registerForm.get("username").value,
      role: "regular",
      email: this.registerForm.get("email").value,
      password: this.registerForm.get("password").value,
      address: this.registerForm.get("address").value,
      firstName: this.registerForm.get("firstName").value,
      lastName: this.registerForm.get("lastName").value,
      age: this.registerForm.get("age").value,
      sports: this.buildSports(this.registerForm.get("sports").value)
    }
  }

  buildSports(sports: any): string {
    var final = "";
    for(let i = 0; i < sports.length; i++) {
      final = final + sports[i].item_text + ';'
    }
    return final;
  }
}
