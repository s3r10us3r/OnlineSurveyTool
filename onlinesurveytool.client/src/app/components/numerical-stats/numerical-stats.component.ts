import {Component, Input, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import {AnswerStatistics, Dataset, NumericalAnswerStatistics} from "../../models/stats";
import {ChartConfig} from "../question-stats/question-stats.component";
import {Chart} from "chart.js";

@Component({
  selector: 'numerical-stats',
  templateUrl: 'numerical-stats.component.html',
  styleUrl: 'numerical-stats.component.css'
})
export class NumericalStatsComponent implements OnInit, OnChanges{
  @Input() oStats!: AnswerStatistics;
  stats!: NumericalAnswerStatistics;
  lineConfig: ChartConfig | null = null;
  constructor() {}

  ngOnInit() {
    this.setUp();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['oStats']) {
      this.setUp();
    }
  }

  setUp() {
    this.stats = this.oStats as NumericalAnswerStatistics;
    const data = this.prepareLineChartData();
    this.lineConfig = this.prepareLineChartConfig(data);
  }

  prepareLineChartConfig(preparedData: Dataset): ChartConfig {
    return {
      type: 'line',
      labels: preparedData.labels,
      dataset: preparedData.dataset,
      options: {
        responsive: true,
        scales: {
          x: {
            type: 'linear',
            position: 'bottom',
            ticks: {
              stepSize: 1
            }
          },
          y: {
            beginAtZero: true
          }
        },
      },
      legend: false,
    };
  }

  prepareLineChartData(): any {
    const labels: string[] = [];
    const data = [];
    for (let ac of this.stats.countOnAnswers) {
      data.push({x: ac.answer, y: ac.count});
    }

    const dataset = {
      data: data,
      label: ''
    }

    return {
      dataset: [dataset],
      labels: labels
    }
  }
}
