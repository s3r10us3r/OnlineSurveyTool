import {
  Component,
  EventEmitter,
  Output,
  Renderer2,
} from '@angular/core';
import {ChoiceOption} from "../../models/choice.option";
import {Question} from "../../models/question";

@Component({
  selector: 'new-multiple-choice',
  templateUrl: './new-multiple-choice.component.html',
  styleUrl: './new-multiple-choice.component.css'
})
export class NewMultipleChoiceComponent{
  @Output() onChange = new EventEmitter<Partial<Question>>();

  constructor(private renderer: Renderer2) {}

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

  autoResize(elem: HTMLTextAreaElement) {
    elem.rows = 1;
    this.renderer.setStyle(elem, 'height', 'auto');
    while (elem.clientHeight < elem.scrollHeight) {
      elem.rows += 1;
    }
  }

}
