import { Token } from "@angular/compiler";

export class User {
  constructor(username: string) {
    this.username = username;
  }

  username: string;
  role: string;
  token: Token;
  mutedOn: Date;
  mutedFor: number;
  banned: boolean;
}
