import {Component, Input, OnInit} from "@angular/core";
import {AnswerStatistics, Dataset, TextualAnswerStatistics} from "../../models/stats";
import {ColorService} from "../../services/color.service";
import {ChartConfig} from "../question-stats/question-stats.component";

@Component({
  selector: 'textual-stats',
  templateUrl: 'textual-stats.component.html',
  styleUrl: 'textual-stats.component.css'
})
export class TextualStatsComponent implements OnInit{
  @Input() oStats!: AnswerStatistics;
  stats!: TextualAnswerStatistics;

  constructor(private colorService: ColorService) {}
  barConfig: ChartConfig | null = null;

  ngOnInit() {
    this.stats = this.oStats as TextualAnswerStatistics;
    console.debug('stats', this.stats)
    this.barConfig = this.prepareBarChartConfig();
    console.debug('config', this.barConfig);
  }

  prepareBarChartConfig(): ChartConfig {
    const data = this.prepareBarChartData();
    return {
      type: 'bar',
      labels: data.labels,
      dataset: data.dataset,
      options: {responsive: true},
      legend: false
    }
  }

  prepareBarChartData(): Dataset {
    const labels: string[] = [];
    const data: number[] = [];
    for (let key in this.stats.mostCommonWords) {
      console.debug(key, this.stats.mostCommonWords[key])
      labels.push(key);
      data.push(this.stats.mostCommonWords[key]);
    }
    const colors = this.colorService.assignColors(data.length);

    return {
      dataset: [{
        data: data,
        label: '',
        backgroundColor: colors
      }],
      labels: labels
    }
  }

}
