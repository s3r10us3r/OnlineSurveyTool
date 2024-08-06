import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ChoiceOption} from "../../models/choice.option";
import {Question, questionPrototype, QuestionType} from "../../models/question";

@Component({
  selector: 'new-single-choice',
  templateUrl: './new-single-choice.component.html',
  styleUrl: './new-single-choice.component.css'
})
export class NewSingleChoiceComponent{
  @Output() onChange = new EventEmitter<Partial<Question>>();

  constructor() {}

  choiceOptions: Array<ChoiceOption> = [];

  addCo() {
    this.choiceOptions.push({number: this.choiceOptions.length, value: ''});
    this.emitChange();
  }

  removeCo(num: number) {
    this.choiceOptions.splice(num, 1);
    this.updateNumbers();
    this.emitChange();
  }

  moveDown(num: number) {
    [this.choiceOptions[num], this.choiceOptions[num + 1]] = [this.choiceOptions[num + 1], this.choiceOptions[num]];
    this.updateNumbers();
    this.emitChange();
  }

  moveUp(num: number) {
    [this.choiceOptions[num], this.choiceOptions[num - 1]] = [this.choiceOptions[num - 1], this.choiceOptions[num]];
    this.updateNumbers();
    this.emitChange();
  }

  updateNumbers(){
    this.choiceOptions.forEach((co, i) => {
      co.number = i;
    });
  }

  emitChange() {
    this.onChange.emit({
      choiceOptions: this.choiceOptions
    })
  }
}
