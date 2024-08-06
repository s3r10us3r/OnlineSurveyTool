import {Component, EventEmitter, Input, Output} from "@angular/core"
import {FormControl, FormGroup} from "@angular/forms";
import {Question, questionPrototype, QuestionType} from "../../models/question";

interface IType{
  display: string,
  value: QuestionType
}

@Component({
  selector: 'new-question-component',
  templateUrl: 'new.question.component.html',
  styleUrl: 'new.question.component.css'
})
export class NewQuestionComponent {
  @Input() number: number = 0;
  @Output() questionChange = new EventEmitter<Question>();
  question: Question = questionPrototype(this.number, QuestionType.SingleChoice);
  constructor() {}

  chosenType: number = 0;
  types: IType[] = [
    {display: 'single choice question', value: QuestionType.SingleChoice},
    {display: 'multiple choice question', value: QuestionType.MultipleChoice},
    {display: 'integer question', value: QuestionType.NumericalInteger},
    {display: 'numerical question', value: QuestionType.NumericalDouble},
    {display: 'textual question', value: QuestionType.Textual},
  ];

  newQuestionForm = new FormGroup({
    value: new FormControl(''),
  })

  switchTypeLeft() {
    this.chosenType--
    if (this.chosenType <= -1) {
      this.chosenType = this.types.length - 1;
    }
    this.question = questionPrototype(this.number, this.types[this.chosenType].value)
    this.sendQuestion();
  }

  switchTypeRight() {
    this.chosenType++
    if (this.chosenType >= this.types.length) {
      this.chosenType = 0;
    }
    this.question = questionPrototype(this.number, this.types[this.chosenType].value)
    this.sendQuestion();
  }

  sendQuestion() {
    this.questionChange.emit(this.question);
  }

  mapProperties(o: Partial<Question>) {
    this.question.choiceOptions = o.choiceOptions !== undefined ? o.choiceOptions : this.question.choiceOptions;
    this.question.minimum = o.minimum !== undefined ? o.minimum : this.question.minimum;
    this.question.maximum= o.maximum!== undefined ? o.maximum: this.question.maximum;
  }

  protected readonly QuestionType = QuestionType;
}
