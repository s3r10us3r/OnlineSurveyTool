import { Component, Input } from "@angular/core";
import {SingleChoiceAnswerStatistics} from "../../models/stats";

@Component({
  selector: 'single-choice-stats',
  templateUrl: 'single-choice-question-stats.component.html',
  styleUrl: 'single-choice-question-stats.component.css'
})
export class SingleChoiceQuestionStatsComponent{
  @Input() stats!: SingleChoiceAnswerStatistics;

  castToSingleChoice(): SingleChoiceAnswerStatistics {
    return this.stats as SingleChoiceAnswerStatistics
  }
}
