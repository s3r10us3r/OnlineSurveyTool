import {Component, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {Question, QuestionType} from "../../models/question";
import {NgModel} from '@angular/forms';

@Component({
  selector: 'numerical-question',
  templateUrl: 'numerical-question.component.html',
  styleUrl: 'numerical-question.component.css'
})
export class NumericalQuestionComponent {
  @Input() question!: Question | null;
  @Output() num = new EventEmitter<number>();
  @ViewChild('numberInput') numberInput!: NgModel;
  @Input() disabled: boolean = false;

  isValid: boolean = false;
  numberValue!: number;

  processNumber(val: number): void {
    this.isValid = this.checkValidity(val);

    console.debug('Number processed', this.isValid, val);

    if (this.isValid) {
      this.num.emit(val);
      return;
    }
    this.num.emit(undefined);
  }

  checkValidity(val: number): boolean {
    return val !== null && val >= this.question!.minimum! && val <= this.question!.maximum! && this.isValueInteger(val);
  }

  isValueInteger(val: number): boolean {
    return this.question!.type !== QuestionType.NumericalInteger || val % 1 === 0
  }

  protected readonly QuestionType = QuestionType;
}
