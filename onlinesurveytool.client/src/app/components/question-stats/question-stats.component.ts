import {Component, Input} from '@angular/core'
import {AnswerStatistics, SingleChoiceAnswerStatistics} from "../../models/stats";

@Component({
  selector: 'question-stats',
  templateUrl: 'question-stats.component.html',
  styleUrl: 'question-stats.component.css'
})
export class QuestionStatsComponent {
  @Input() stats!: AnswerStatistics;

  castToSingleChoice(): SingleChoiceAnswerStatistics {
    return this.stats as SingleChoiceAnswerStatistics;
  }
}
