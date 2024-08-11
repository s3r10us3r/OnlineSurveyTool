import {
  AfterContentInit, AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Renderer2,
  ViewChild
} from "@angular/core"
import {FormControl, FormGroup} from "@angular/forms";
import {Question, questionPrototype, QuestionType} from "../../models/question";

interface IType{
  display: string,
  value: QuestionType
}

@Component({
  selector: 'new-question-component',
  templateUrl: 'new.question.component.html',
  styleUrl: 'new.question.component.css',
})
export class NewQuestionComponent implements AfterViewInit{
  @Input() question: Question = questionPrototype(QuestionType.SingleChoice);
  @Output() questionChange = new EventEmitter<Question>();

  @ViewChild('questionValue') questionValue!: ElementRef;

  constructor(private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.autoResize();
  }

  chosenType: number = 0;

  newQuestionForm = new FormGroup({
    value: new FormControl(''),
  })


  switchTypeLeft() {
    this.chosenType--
    if (this.chosenType <= -1) {
      this.chosenType = this.types.length - 1;
    }
    this.question.type = this.prevType(this.question.type);
    this.sendQuestion();
  }

  switchTypeRight() {
    if (this.chosenType >= this.types.length) {
      this.chosenType = 0;
    }
    this.question.type = this.nextType(this.question.type);
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

  protected readonly QuestionType = QuestionType;
}
