import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {SurveyService} from "../../services/survey.service";
import {Survey} from "../../models/survey";
import {Result} from "../../models/result";
import {Answer} from "../../models/answer";
import {Question} from "../../models/question";
import {ResultService} from "../../services/result.service";

@Component({
  selector: 'answer-page',
  templateUrl: 'answer-page.component.html',
  styleUrl: 'answer-page.component.css'
})
export class AnswerPageComponent implements OnInit{
  survey: Survey | null = null;
  result!: Result;
  isValid: boolean = false;
  answers: Array<Answer | null> = [];

  constructor(
    private route: ActivatedRoute,
    private surveyService: SurveyService,
    private router: Router,
    private resultService: ResultService) {}

  ngOnInit(): void {
    const surveyId = this.route.snapshot.paramMap.get('id');

    if (surveyId === null) {
      this.router.navigate(['/error']);
      return;
    }

    this.surveyService.getSurvey(surveyId).subscribe({
      next: res => {
        this.survey = res;
        this.survey.questions.sort((a, b) => a.number - b.number);
        this.result = {
          id: this.survey!.id!,
          answers: Array(this.survey!.questions.length).fill(undefined)
        };
        this.answers = Array(this.survey!.questions.length).fill(undefined);
      },
      error: err => {console.error(err); this.router.navigate(['/error'])}
    });
  }

  updateResult(answer: Answer | null, number: number) {
    this.answers[number] = answer;
    this.isValid = this.validateResult();
  }

  validateResult(): boolean {
    for (let [i, ans] of this.answers.entries()) {
      if (ans === undefined) {
        console.debug('question', i, 'is invalid', this.result);
        return false;
      }
    }
    return true;
  }

  submit(): void {
    if (!this.isValid)
      return;
    this.constructResult();
    this.resultService.submitResult(this.result).subscribe({
      next: res => this.router.navigate(['/submitted']),
      error: err => {console.error(err); this.router.navigate(['/error'])}
    })
  }

  constructResult(): void {
    // @ts-ignore
    this.result.answers = this.answers.filter(a => a);
  }
}
