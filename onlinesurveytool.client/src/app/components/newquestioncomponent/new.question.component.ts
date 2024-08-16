import {
  OnChanges, AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  Renderer2,
  ViewChild
} from "@angular/core"
import {FormControl, FormGroup} from "@angular/forms";
import {Question, questionPrototype, QuestionType} from "../../models/question";

export interface ErrorObj{
  num: number,
  value: boolean
}

@Component({
  selector: 'new-question-component',
  templateUrl: 'new.question.component.html',
  styleUrl: 'new.question.component.css',
})
export class NewQuestionComponent implements AfterViewInit, OnChanges{
  @Input() question: Question = questionPrototype(QuestionType.SingleChoice);
  @Output() questionChange = new EventEmitter<Question>();
  @Output() error = new EventEmitter<ErrorObj>();

  @ViewChild('questionValue') questionValue!: ElementRef;

  constructor(private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.autoResize();
    this.checkErrors();
  }

  ngOnChanges() {
    this.checkErrors();
  }

  chosenType: number = 0;
  errorMessage: string = '';
  childError: string = '';
  isEmpty: boolean = true;

  newQuestionForm = new FormGroup({
    value: new FormControl(''),
  })


  switchTypeLeft() {
    this.childError = '';
    this.chosenType--
    if (this.chosenType <= -1) {
      this.chosenType = this.types.length - 1;
    }
    this.question.type = this.prevType(this.question.type);
    this.sendQuestion();
    this.checkErrors();
  }

  switchTypeRight() {
    this.childError = '';
    if (this.chosenType >= this.types.length) {
      this.chosenType = 0;
    }
    this.question.type = this.nextType(this.question.type);
    this.sendQuestion();
    this.checkErrors();
  }

  sendQuestion() {
    this.questionChange.emit(this.question);
  }

  mapProperties(o: Partial<Question>) {
    this.question.choiceOptions = o.choiceOptions !== undefined ? o.choiceOptions : this.question.choiceOptions;
    this.question.minimum = o.minimum !== undefined ? o.minimum : this.question.minimum;
    this.question.maximum= o.maximum!== undefined ? o.maximum: this.question.maximum;
  }


  types: QuestionType[] = [
    QuestionType.SingleChoice, QuestionType.MultipleChoice, QuestionType.NumericalInteger, QuestionType.NumericalDouble,
    QuestionType.Textual
  ];

  nextType(type: QuestionType): QuestionType {
    let ind = this.types.findIndex(t => t === type);
    ind += 1;
    if ( ind >= this.types.length ) {
      ind = 0;
    }
    return this.types[ind];
  }

  prevType(type: QuestionType): QuestionType {
    let ind = this.types.findIndex(t => t === type);
    ind -= 1;
    if ( ind < 0) {
      ind = this.types.length - 1;
    }
    return this.types[ind];
  }

  getTypeDisplayName(type: QuestionType) {
    const displayNames = {
      'Single Choice': 'single choice',
      'Multiple Choice': 'multiple choice',
      'Numerical Integer': 'numerical integer question',
      'Numerical Double': 'numerical decimal question',
      'Textual': 'textual question'
    };
    return displayNames[type];
  }

  autoResize() {
    const nativeElem = this.questionValue.nativeElement;
    this.renderer.setStyle(nativeElem, 'height', 'auto');
    this.renderer.setStyle(nativeElem, 'height', nativeElem.scrollHeight  + 'px');
  }

  checkValue() {
    const val = this.questionValue.nativeElement.value;
    this.isEmpty = !val;
    this.checkErrors();
  }

  processError(error: string): void {
    this.childError = error;
    this.checkErrors();
  }

  checkErrors() {
    this.errorMessage = '';
    if (this.isEmpty) {
      this.errorMessage = 'Question must not be empty!';
    }

    if (this.childError) {
      this.errorMessage = this.childError;
    }
    if (this.errorMessage)
      this.emitError();
    else
      this.emitNoError();
  }

  emitError() {
    this.error.emit({
      num: this.question.number,
      value: true
    });
  }

  emitNoError() {
    this.error.emit({
      num: this.question.number,
      value: false
    });
  }


  protected readonly QuestionType = QuestionType;
}
