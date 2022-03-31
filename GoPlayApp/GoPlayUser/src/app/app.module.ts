import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AngularFireStorageModule } from '@angular/fire/compat/storage';
import { AngularFireModule } from '@angular/fire/compat';
import { environment } from '../environments/environment';
import { NgToggleModule } from '@nth-cloud/ng-toggle';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';

import { AppComponent } from './app.component';
import { IndexComponent } from './index/index.component';
import { NavComponent } from './nav/nav.component';
import { LocalStorageService } from './services/local-storage.service';
import { RegisterComponent } from './register/register.component';
import { RegisterCenterComponent } from './register-center/register-center.component';
import { HomeComponent } from './home/home.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { MessagingComponent } from './messaging/messaging.component';
import { UserServiceService } from './services/user.service';
import { RequestInterceptor } from './Interceptors/RequestInterceptor';
import { ReportComponent } from './report/report.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    NavComponent,
    RegisterComponent,
    RegisterCenterComponent,
    HomeComponent,
    CreatePostComponent,
    MessagingComponent,
    ReportComponent,
    ConfirmEmailComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      closeButton: true,
      newestOnTop: true,
      progressBar: true,
      positionClass: "toast-bottom-right",
      preventDuplicates: true,
      timeOut: 3000,
      extendedTimeOut: 1000
    }),
    AngularFireStorageModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    NgToggleModule,
    NgMultiSelectDropDownModule.forRoot(),
    MatDatepickerModule,
    MatNativeDateModule
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
