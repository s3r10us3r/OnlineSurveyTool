import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/loginpage/loginpage.component';
import { RegisterComponent } from './components/registerpage/registerpage.component';
import {ErrorPageComponent} from "./components/errorpage/errorpage.component";
import {MainPageComponent} from "./components/mainpage/main.page.component";
import {canUseRoute} from "./services/auth.service";

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent},
  { path: 'error', component: ErrorPageComponent},
  { path: 'main', component: MainPageComponent, canActivate: [canUseRoute]},
  { path: '**', redirectTo: '/error'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
