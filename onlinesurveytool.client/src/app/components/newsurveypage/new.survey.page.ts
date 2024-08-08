import {Component} from "@angular/core";
import {Question, questionPrototype, QuestionType} from "../../models/question";

@Component({
  selector: 'new-survey-page',
  templateUrl: 'new.survey.page.html',
  styleUrls: ['../../styles/userForm.css', 'new.survey.page.css']
})
export class NewSurveyPage {
  constructor() {}

  questions: Array<Question> = []

  addQuestion() {
    this.questions.push(questionPrototype(QuestionType.SingleChoice, this.questions.length));
  }

  getQuestion(question: Question) {
    this.questions[question.number] = question;
  }

  removeQuestion(ind: number) {
    this.questions.splice(ind, 1)
  }
}
