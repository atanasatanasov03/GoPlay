import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable, ɵɵsetComponentScope } from "@angular/core";
import { Observable } from "rxjs";
import { Token } from "../models/Token";
import { LocalStorageService } from "../services/local-storage.service";
import { UserServiceService } from "../services/user.service";

@Injectable()
export class RequestInterceptor implements HttpInterceptor {
  token: Token
  constructor(private lsService: LocalStorageService, private userService: UserServiceService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.token = this.lsService.getToken();
    try {
      var authHeader = 'Bearer ' + this.token.token;//this.token.access_token;
      const authReq = req.clone({ setHeaders: { Authorization: authHeader } });

      return next.handle(authReq);
    }
    catch
    {
      return next.handle(req);
    }
  }
}
