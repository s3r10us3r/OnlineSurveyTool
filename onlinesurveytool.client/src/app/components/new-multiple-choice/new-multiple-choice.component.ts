import {
  Component,
  EventEmitter, OnInit,
  Output,
  Renderer2,
} from '@angular/core';
import {ChoiceOption} from "../../models/choice.option";
import {Question} from "../../models/question";
import {FormControl, FormGroup} from "@angular/forms";
import {NumberService} from "../../services/number.service";
import {max, min} from "rxjs";

@Component({
  selector: 'new-multiple-choice',
  templateUrl: './new-multiple-choice.component.html',
  styleUrl: './new-multiple-choice.component.css'
})
export class NewMultipleChoiceComponent implements OnInit{
  @Output() onChange = new EventEmitter<Partial<Question>>();
  @Output() errorChange = new EventEmitter<string>();

  constructor(private renderer: Renderer2, private numberService: NumberService) {
  }

  ngOnInit(): void {
    this.errorChange.emit('There must be at least one choice option!');
  }

  minimum: number = 0;
  maximum: number = 0;
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

  updateNumbers() {
    this.choiceOptions.forEach((co, i) => {
      co.number = i;
    });
  }

  emitChange() {
    if (this.choiceOptions.length === 0) {
      this.errorChange.emit('There must be at least one choice option!');
      return;
    }

    this.errorChange.emit('');
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

  protected readonly max = max;
  protected readonly Math = Math;
}
