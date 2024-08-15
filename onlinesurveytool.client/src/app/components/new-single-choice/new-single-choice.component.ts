import {
  Component,
  EventEmitter, OnInit,
  Output,
  Renderer2,
} from '@angular/core';
import {ChoiceOption} from "../../models/choice.option";
import {Question, questionPrototype, QuestionType} from "../../models/question";

@Component({
  selector: 'new-single-choice',
  templateUrl: './new-single-choice.component.html',
  styleUrl: './new-single-choice.component.css'
})
export class NewSingleChoiceComponent implements OnInit{
  @Output() onChange = new EventEmitter<Partial<Question>>();
  @Output() error = new EventEmitter<string>();

  constructor(private renderer: Renderer2) {}

  ngOnInit(): void {
    this.error.emit('There must be at least one choice option!');
  }

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
    if (this.choiceOptions.length === 0) {
      this.error.emit('There must be at least one choice option.');
      return;
    }

    this.error.emit('');
    this.onChange.emit({
      choiceOptions: this.choiceOptions
    })
  }

  autoResize(elem: HTMLTextAreaElement) {
    elem.rows = 1;
    this.renderer.setStyle(elem, 'height', 'auto');
    while (elem.clientHeight < elem.scrollHeight) {
      elem.rows += 1;
    }
  }

}
