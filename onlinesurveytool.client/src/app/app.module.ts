import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/loginpage/loginpage.component';
import { RegisterComponent } from './components/registerpage/registerpage.component'
import {ErrorPageComponent} from "./components/errorpage/errorpage.component";
import {MainPageComponent} from "./components/mainpage/main.page.component";
import {NewSurveyPage} from "./components/newsurveypage/new.survey.page";
import {AuthService} from "./services/auth.service";
import {UserService} from "./services/user.service";
import {NewQuestionComponent} from "./components/newquestioncomponent/new.question.component";
import {NewSingleChoiceComponent} from "./components/new-single-choice/new-single-choice.component";
import {NewMultipleChoiceComponent} from "./components/new-multiple-choice/new-multiple-choice.component";

@NgModule({
  declarations: [
    AppComponent, LoginComponent, RegisterComponent,
    ErrorPageComponent, MainPageComponent, NewSurveyPage,
    NewQuestionComponent, NewSingleChoiceComponent, NewMultipleChoiceComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, ReactiveFormsModule, FormsModule
  ],
  providers: [AuthService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
