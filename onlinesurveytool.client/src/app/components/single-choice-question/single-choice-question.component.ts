import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Question, QuestionType } from "../../models/question";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: 'single-choice-question',
  templateUrl: 'single-choice-question.component.html',
  styleUrls: ['single-choice-question.component.css']
})
export class SingleChoiceQuestionComponent implements OnInit {
  @Input() question: Question | null = null;
  @Input() disabled: boolean = false;
  @Output() choice = new EventEmitter<number>();

  surveyForm!: FormGroup;
  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.question!.choiceOptions!.sort((a, b) => a.number - b.number);

    this.surveyForm = this.fb.group({
      checkboxes: this.fb.array(this.question!.choiceOptions!.map(() => new FormControl(false)))
    });
    console.debug('single choice question component on init', this.question?.type);
  }

  get checkboxes(): FormArray {
    return this.surveyForm.get('checkboxes') as FormArray;
  }

  onCheckBoxChange(ind: number, num: number): void {
    this.checkboxes.controls.forEach((control, index) => {
      if (index !== ind) {
        control.setValue(false);
      }
    });

    if (this.checkboxes.controls[ind].value) {
      this.choice.emit(num);
      console.debug('emitted', num);
      return;
    }

    this.choice.emit(undefined);
    console.debug('emitted undefined');
  }
}
