import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {Question, QuestionType} from "../../models/question";
import {Answer} from "../../models/answer";

@Component({
  selector: 'answer-box',
  templateUrl: 'answer-box.component.html',
  styleUrl: 'answer-box.component.css'
})
export class AnswerBoxComponent implements OnInit{
  @Input() question: Question | null = null;
  @Output() ans = new EventEmitter<Answer | null>();

  answer!: Answer;
  disabled: boolean = false;
  constructor() {
  }

  ngOnInit() {
    console.debug('question', this.question);
  }

  constructAnswer() {
    this.answer = {
      number: this.question!.number
    }
  }

  handleSingleChoice(choice: number) {
    if (choice < 0) {
      this.ans.emit(undefined);
      return;
    }

    this.constructAnswer();
    this.answer.chosenOptions = [choice];
    this.ans.emit(this.answer);
  }

  handleTextual(text: string | undefined) {
    if (text === undefined) {
      this.ans.emit(undefined);
      return;
    }

    this.constructAnswer();
    this.answer.textAnswer = text;
    this.ans.emit(this.answer);
  }

  handleMultipleChoice(choices: number[] | undefined) {
    if (choices === undefined) {
      this.ans.emit(undefined)
      return;
    }

    this.constructAnswer();
    this.answer.chosenOptions = choices;
    this.ans.emit(this.answer);
  }

  handleNumerical(val: number | undefined): void {
    if (val === undefined) {
      this.ans.emit(undefined);
      return;
    }

    this.constructAnswer();
    this.answer.answer = val;
    this.ans.emit(this.answer);
  }

  skipBoxChange(event: Event) {
    const input = event.target as HTMLInputElement;
    this.disabled = input.checked;
    console.debug(this.disabled);
    if (input.checked) {
      this.ans.emit(null);
      return;
    }
    this.ans.emit(this.answer);
  }

  protected readonly QuestionType = QuestionType;
  protected readonly window = window;
}
