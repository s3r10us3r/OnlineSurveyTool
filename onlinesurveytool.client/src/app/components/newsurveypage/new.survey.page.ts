import {Component, ChangeDetectorRef} from "@angular/core";
import {Question, questionPrototype, QuestionType} from "../../models/question";
import {ErrorObj} from "../newquestioncomponent/new.question.component";
import {Survey} from "../../models/survey";
import {SurveyService} from "../../services/survey.service";
import {Router} from "@angular/router";

@Component({
  selector: 'new-survey-page',
  templateUrl: 'new.survey.page.html',
  styleUrls: ['../../styles/userForm.css', 'new.survey.page.css']
})
export class NewSurveyPage {
  constructor(private changeDetector: ChangeDetectorRef,
              private surveyService: SurveyService,
              private router: Router) {}

  questions: Array<Question> = [];
  errors: Array<boolean> = [];
  canBeSent: boolean = false;
  surveyName: string = '';

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
    this.canBeSent = this.checkIfCanBeSent();
    this.changeDetector.detectChanges();
  }

  checkIfCanBeSent(): boolean {
    const isError = this.checkIfThereIsError();
    const isEmpty = this.questions.length <= 0;

    return !isError && !isEmpty;
  }

  checkIfThereIsError(): boolean {
    for (let err of this.errors) {
      if (err)
        return true;
    }
    return false;
  }

  submitSurvey() {
    const survey = this.constructSurvey();
    console.debug('sending request...');
    this.surveyService.addSurvey(survey)
      .subscribe({
        next: result => this.handleResult(result),
        error: error => this.handleError(error),
      })
    console.debug(survey);
  }

  handleResult(result: object) {
    console.debug(result);
    this.router.navigate(['/main']);
  }

  handleError(error: object) {
    console.error(error);
    this.router.navigate(['/error']);
  }

  constructSurvey(): Survey {
    return {
      name: this.surveyName,
      questions: this.questions,
    };
  }

  goBack(): void {
    this.router.navigate(['/main']);
  }
}
