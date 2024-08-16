import {Component} from "@angular/core";
import {Question, questionPrototype, QuestionType} from "../../models/question";
import {ErrorObj} from "../newquestioncomponent/new.question.component";

@Component({
  selector: 'new-survey-page',
  templateUrl: 'new.survey.page.html',
  styleUrls: ['../../styles/userForm.css', 'new.survey.page.css']
})
export class NewSurveyPage {
  constructor() {}

  questions: Array<Question> = [];
  errors: Array<boolean> = [];

  addQuestion() {
    this.errors.push(false);
    this.questions.push(questionPrototype(QuestionType.SingleChoice, this.questions.length));
  }

  getQuestion(question: Question) {
    this.questions[question.number] = question;
  }

  removeQuestion(ind: number) {
    this.errors.splice(ind, 1);
    this.questions.splice(ind, 1)
  }

  processError(err: ErrorObj) {
    this.errors[err.num] = err.value;
  }

  canBeSent(): boolean {
    return !!this.errors.find(e => e); //the !! is only to satisfy ts
  }
}
