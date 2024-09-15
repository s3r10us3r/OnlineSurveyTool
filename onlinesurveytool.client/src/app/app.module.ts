import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

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
import {NumberService} from "./services/number.service";
import {CoMinMaxComponent} from "./components/co-min-max/co-min-max.component";
import {MinMaxQuestionComponent} from "./components/min-max-question-component/min-max-question.component";
import {SurveyService} from "./services/survey.service";
import {AuthInterceptor} from "./services/auth.interceptor";
import {ResultHeaderCardComponent} from "./components/result-header-card/result-header-card.component";
import {HeaderListComponent} from "./components/header-list/header-list.component";
import {AnswerPageComponent} from "./components/answer-page/answer-page.component";
import {AnswerBoxComponent} from "./components/answer-box/answer-box.component";
import {SingleChoiceQuestionComponent} from "./components/single-choice-question/single-choice-question.component";
import {TextualQuestionComponent} from "./components/textual-question/textual-question.component";
import {
  MultipleChoiceQuestionComponent
} from "./components/multiple-choice-question/multiple-choice-question.component";
import {NumericalQuestionComponent} from "./components/numerical-question/numerical-question.component";
import {ResultService} from "./services/result.service";
import {SubmittedPageComponent} from "./components/submitted-page/submitted-page.component";
import {StatsPageComponent} from "./components/stats-page/stats-page.component";
import { StatsService } from './services/stats.service';
import {QuestionStatsComponent} from "./components/question-stats/question-stats.component";
import {
  SingleChoiceQuestionStatsComponent
} from "./components/single-choice-question-stats/single-choice-question-stats.component";

@NgModule({
  declarations: [
    AppComponent, LoginComponent, RegisterComponent,
    ErrorPageComponent, MainPageComponent, NewSurveyPage,
    NewQuestionComponent, NewSingleChoiceComponent, NewMultipleChoiceComponent,
    CoMinMaxComponent, MinMaxQuestionComponent, ResultHeaderCardComponent,
    HeaderListComponent, AnswerPageComponent, AnswerBoxComponent,
    SingleChoiceQuestionComponent, TextualQuestionComponent, MultipleChoiceQuestionComponent,
    NumericalQuestionComponent, SubmittedPageComponent, StatsPageComponent,
    QuestionStatsComponent, SingleChoiceQuestionStatsComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, ReactiveFormsModule, FormsModule
  ],
  providers: [AuthService, UserService, NumberService, SurveyService, ResultService, StatsService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
