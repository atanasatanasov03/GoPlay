import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageServiceService } from '../services/message-service.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-messaging',
  templateUrl: './messaging.component.html',
  styleUrls: ['./messaging.component.css']
})
export class MessagingComponent implements OnInit {
  title = 'chat-ui';
  text: string = "";
  groupName: string;
  joinedGroup: boolean = false;

  constructor(public messageService: MessageServiceService, public userService: UserServiceService, public router: Router) { }

  ngOnInit(): void {
    if (!this.userService.isLogged()) this.router.navigate([""])
    this.messageService.connect();
  }

  joinGroup() {
    this.messageService.joinGroup(this.groupName);
    this.joinedGroup = true;
  }

  sendMessage(): void {
    this.messageService.sendMessageToGroup(this.groupName, this.text).subscribe(_ => {
      this.text = '';
      console.log("group message recieved in component")
    });

    /*this.messageService.sendMessageToApi(this.text).subscribe({
       next: _ => this.text = '',
       error: (err) => console.error(err)
     });

    this.messageService.sendMessageToHub(this.text).subscribe({
      next: _ => this.text = '',
      error: (err) => console.error(err)
    });*/

  }
}
