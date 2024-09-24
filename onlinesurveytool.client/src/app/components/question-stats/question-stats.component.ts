import {Component, Input, OnChanges, SimpleChanges} from '@angular/core'
import {
  AnswerStatistics,
  NumericalAnswerStatistics,
  SingleChoiceAnswerStatistics,
  TextualAnswerStatistics
} from "../../models/stats";

@Component({
  selector: 'question-stats',
  templateUrl: 'question-stats.component.html',
  styleUrl: 'question-stats.component.css'
})
export class QuestionStatsComponent {

  @Input() stats!: AnswerStatistics;
  @Input() resultCount: number = 0;

  castToSingleChoice(stats: AnswerStatistics): SingleChoiceAnswerStatistics {
    return stats as SingleChoiceAnswerStatistics;
  }

  castToTextual(stats: AnswerStatistics): TextualAnswerStatistics {
    console.debug(this.stats);
    return this.stats as TextualAnswerStatistics;
  }

  castToNumerical(stats: AnswerStatistics): NumericalAnswerStatistics {
    return this.stats as NumericalAnswerStatistics
  }

  getAnsweredPercentage(): number {
    return parseFloat(((this.stats.numberOfAnswers / this.resultCount) * 100).toFixed(2));
  }
}

export interface ChartConfig {
  type: "bar" | "line" | "scatter" | "bubble" | "pie" | "doughnut" | "polarArea" | "radar",
  dataset: [{ data: number[], label: string}],
  labels: string[],
  options: object,
  legend: boolean,
}
