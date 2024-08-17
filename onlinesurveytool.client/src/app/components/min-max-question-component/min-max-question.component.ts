import {Component, EventEmitter, Input, Output, OnInit} from "@angular/core";
import {Question} from "../../models/question";
import {NumberService} from "../../services/number.service";

@Component({
  selector: 'min-max-question-component',
  templateUrl: 'min-max-question.component.html',
  styleUrl: 'min-max-question.component.css'
})
export class MinMaxQuestionComponent implements OnInit {
  @Input() minText!: string;
  @Input() maxText!: string;
  @Input() min!: number;
  @Input() max!: number;
  @Input() numType!: 'decimal' | 'double';

  @Output() onChange = new EventEmitter<Partial<Question>>();
  @Output() errorChange = new EventEmitter<string>();

  minValue: string = '';
  maxValue: string = '';

  constructor(private numberService: NumberService) {}

  ngOnInit(): void {
    this.emitValues();
  }

  updateMin(event: Event) {
    this.minValue = this.extractValueFromEvent(event);
    this.emitValues();
  }

  updateMax(event: Event) {
    this.maxValue = this.extractValueFromEvent(event);
    this.emitValues();
  }

  extractValueFromEvent(event: Event) {
    const inputElem = event.target as HTMLInputElement;
    return inputElem.value;
  }

  emitValues() {
    if (this.minValue === '' || this.maxValue === '') {
      this.errorChange.emit('Inputs cannot be empty!');
      return;
    }

    const err1 = this.checkValue(this.minValue);
    if (err1) {
      this.errorChange.emit(err1);
      return;
    }

    const err2 = this.checkValue(this.maxValue);
    if (err2) {
      this.errorChange.emit(err2);
      return;
    }

    const [min, max] = this.parseNums();
    if (max < min) {
      this.errorChange.emit('Minimum must be higher than maximum!');
      return;
    }

    this.errorChange.emit('');

    const question = this.makeQuestion(min, max);
    this.onChange.emit(question);
  }

  makeQuestion(minNum: number, maxNum: number): Partial<Question>{
    return {
      minimum: minNum,
      maximum: maxNum
    }
  }


  parseNums(): [number, number] {
    return [Number(this.minValue), Number(this.maxValue)];
  }

  checkValue(val: string): string {
    if (this.numType === 'decimal') {
      if (this.numberService.isValidInteger(val)) {
        return '';
      }
      return 'Both numbers must be valid integers!';
    }

    if (this.numberService.isValidDouble(val)) {
      return '';
    }
    return 'Both numbers must be valid!';
  }
}
