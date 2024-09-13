import { Component, EventEmitter, Input, Output } from '@angular/core';
import {Question} from "../../models/question";
import {FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'multiple-choice-question',
  templateUrl: 'multiple-choice-question.component.html',
  styleUrl: 'multiple-choice-question.component.css'
})
export class MultipleChoiceQuestionComponent {
  @Input() question: Question | null = null;
  @Output() choices = new EventEmitter<number[]>();
  @Input() disabled: boolean = false;

  isValid: boolean = false;

  surveyForm!: FormGroup;
  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.question!.choiceOptions!.sort((a, b) => a.number - b.number);

    this.surveyForm = this.fb.group({
      checkboxes: this.fb.array(this.question!.choiceOptions!.map(() => new FormControl(false)))
    });

    this.isValid = this.question!.minimum === 0;
    console.debug('single choice question component on init', this.question?.type);
  }

  get checkboxes(): FormArray {
    return this.surveyForm.get('checkboxes') as FormArray;
  }

  onCheckBoxChange(ind: number, num: number): void {
    let res: number[] = [];

    this.checkboxes.controls.forEach((control, index) => {
      control.enable();
      if (control.value) {
        res.push(index);
      }
    });

    const disabled = res.length >= this.question!.maximum!;
    this.checkboxes.controls.forEach((control, index) => {
      if (disabled && !control.value)
        control.disable();
    });

    if (res.length >= this.question!.minimum!) {
      this.choices.emit(res);
      this.isValid = true;
      return;
    }

    this.choices.emit(undefined);
    this.isValid = false;
  }


}
