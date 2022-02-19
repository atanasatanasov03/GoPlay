import { Token } from "@angular/compiler";

export class User {
  constructor(username: string) {
    this.username = username;
  }

  username: string;
  token: Token;
}
