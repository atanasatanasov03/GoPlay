import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { IndexComponent } from "./index/index.component";
import { RegisterCenterComponent } from "./register-center/register-center.component";

const routes: Routes = [
  { path: '', component: IndexComponent },
  { path: 'registerCenter', component: RegisterCenterComponent },
  { path: 'home', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
