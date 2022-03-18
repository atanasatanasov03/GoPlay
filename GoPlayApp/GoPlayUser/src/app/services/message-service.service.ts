import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MessagePackHubProtocol } from '@microsoft/signalr-protocol-msgpack'
import { Message } from '../models/Message';
import { UserServiceService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class MessageServiceService {
  private hubConnection: HubConnection;
  public groups: string[];
  public groupName: string;
  public messages: Message[] = [];
  private connectionUrl = 'https://localhost:7170/message';
  private apiUrl = 'https://localhost:7170/chat';

  constructor(private http: HttpClient, private userService: UserServiceService) {
    
  }

  public connect = () => {
    this.startConnection();
    this.addListeners();
    this.recieveMessageFromGroup();
  }

  private getConnection(): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(this.connectionUrl)
      .withHubProtocol(new MessagePackHubProtocol())
      .build();
  }

  private buildChatMessage(message: string, groupname: string): Message {
    return {
      username: this.userService.username,
      groupName: groupname,
      text: message,
      dateTime: new Date()
    };
  }

  private startConnection() {
    this.hubConnection = this.getConnection();
    this.hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing connection: ' + err))
  }

  private addListeners() {
    this.hubConnection.on("messageReceivedFromApi", (data: Message) => {
      console.log("message received from Controller")
      this.messages.push(data);
    })
  }

  public joinGroup() {
    this.getMessagesForGroup(this.groupName).subscribe((ms:Message[]) => this.messages = ms);
    this.hubConnection.invoke("AddToGroup", this.groupName, this.userService.username).catch(err => console.log(err));
  }

  public leaveGroup() {
    this.http.post(this.apiUrl + "/removeFromGroup?username=" + this.userService.username + "&groupname=" + this.groupName, "").subscribe(_ => {
      const index = this.groups.indexOf(this.groupName);
      this.groups.splice(index, 1);
      this.groupName = this.groups[0];
      this.joinGroup();
    });
  }

  public getGroups() {
    this.http.get<string[]>(this.apiUrl + "/getGroupsFor?username=" + this.userService.username).subscribe(r => this.groups = r);
  }

  public getMessagesForGroup(groupName: string) {
    return this.http.get<Message[]>(this.apiUrl + "/getMessagesFor?groupname=" + groupName).pipe(tap(_ => console.log("Fetched messages")))
  }

  public sendMessageToGroup(message: string) {
    this.messages.push(this.buildChatMessage(message, this.groupName))
    return this.http.post(this.apiUrl + "/messageGroup", this.buildChatMessage(message, this.groupName))
      .pipe(tap(_ => console.log("group message sucessfully sent to controller")));
  }

  private recieveMessageFromGroup() {
    this.hubConnection.on("RecieveMessage", (data: Message) => {
      console.log("message received from Group")
      if (data.username != this.userService.username) {
        console.log(data)
        this.messages.push(data);
      }
    })
  }
}