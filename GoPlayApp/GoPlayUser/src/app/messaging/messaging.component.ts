import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'app/services/local-storage.service';
import { MessageServiceService } from '../services/message-service.service';

@Component({
  selector: 'app-messaging',
  templateUrl: './messaging.component.html',
  styleUrls: ['./messaging.component.css']
})
export class MessagingComponent implements OnInit {
  container: HTMLElement;

  title = 'chat-ui';
  text: string = "";
  joinedGroup: boolean = false;

  constructor(public messageService: MessageServiceService, public localStorage: LocalStorageService, public router: Router) { }

  ngOnInit(): void {
    if (this.localStorage.getUser() == null) this.router.navigate([""])
  }

  ngAfterViewInit() {
    this.container = document.getElementById("chat");
    this.container.scrollTop = this.container.scrollHeight;
  }

  openChat(group: string) {
    this.messageService.groupName = group;
    this.messageService.joinGroup();
  }

  leaveGroup() {
    this.messageService.leaveGroup();
  }

  sendMessage(): void {
    this.messageService.sendMessageToGroup(this.text).subscribe(_ => {
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
