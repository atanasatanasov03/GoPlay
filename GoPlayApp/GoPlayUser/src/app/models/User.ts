import { Token } from "@angular/compiler";

export class User {
  constructor(username: string) {
    this.userName = username;
  }

  userName: string;
  role: string;
  token: Token;
  mutedOn: string;
  mutedFor: number;
  banned: boolean;
  verified: boolean;
}
