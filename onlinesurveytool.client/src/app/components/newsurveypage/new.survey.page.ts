import {Component} from "@angular/core";
import {Question} from "../../models/question";

@Component({
  selector: 'new-survey-page',
  templateUrl: 'new.survey.page.html',
  styleUrls: ['../../styles/userForm.css', 'new.survey.page.css']
})
export class NewSurveyPage {
  constructor() {}

  questions: Array<Question | null> = []
  addQuestion() {
    this.questions.push(null);
  }

  getQuestion(question: Question) {
    this.questions[question.number] = question;
  }
}
