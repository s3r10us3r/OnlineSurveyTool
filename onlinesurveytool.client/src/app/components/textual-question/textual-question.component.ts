import { Component, EventEmitter, Input, Output } from '@angular/core'
import {Question} from "../../models/question";
@Component({
  selector: 'textual-question',
  templateUrl: 'textual-question.component.html',
  styleUrl: 'textual-question.component.css'
})
export class TextualQuestionComponent {
  @Input() question: Question | null = null;
  @Output() textInput = new EventEmitter<string>();
  @Input() disabled: boolean = false;

  textLength: number = 0;

  updateText(event: Event): void {
    const text = (event.target as HTMLTextAreaElement).value;
    this.textLength = text.length;
    if (text.length >= this.question?.minimum! && text.length <= this.question?.maximum!) {
      this.textInput.emit(text);
      return;
    }

    this.textInput.emit(undefined);
  }
}
