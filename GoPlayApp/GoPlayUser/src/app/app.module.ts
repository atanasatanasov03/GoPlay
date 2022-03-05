import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { IndexComponent } from './index/index.component';
import { NavComponent } from './nav/nav.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalStorageService } from './services/local-storage.service';
import { RegisterComponent } from './register/register.component';
import { FormsModule } from '@angular/forms';
import { RegisterCenterComponent } from './register-center/register-center.component';
import { HomeComponent } from './home/home.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { MessagingComponent } from './messaging/messaging.component';
import { UserServiceService } from './services/user.service';
import { RequestInterceptor } from './Interceptors/RequestInterceptor';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    NavComponent,
    RegisterComponent,
    RegisterCenterComponent,
    HomeComponent,
    CreatePostComponent,
    MessagingComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptor,
      multi: true
    },
    UserServiceService,
    LocalStorageService
  ],
  bootstrap: [AppComponent],
  entryComponents: [IndexComponent]
})
export class AppModule { }
