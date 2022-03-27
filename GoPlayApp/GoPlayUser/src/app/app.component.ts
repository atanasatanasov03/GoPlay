import { Component, OnInit } from '@angular/core';
import { User } from './models/User';
import { UserServiceService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'GoPlayUser';

  constructor(public userService: UserServiceService) { }

  ngOnInit(){}
}

