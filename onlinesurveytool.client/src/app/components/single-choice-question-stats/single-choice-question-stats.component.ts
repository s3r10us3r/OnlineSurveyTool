import {Component, Input, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import {AnswerStatistics, Dataset, SingleChoiceAnswerStatistics} from "../../models/stats";
import {ChartConfig} from "../question-stats/question-stats.component";
import {ColorService} from "../../services/color.service";

@Component({
  selector: 'single-choice-stats',
  templateUrl: 'single-choice-question-stats.component.html',
  styleUrl: 'single-choice-question-stats.component.css'
})
export class SingleChoiceQuestionStatsComponent implements OnInit, OnChanges{
  @Input() oStats!: AnswerStatistics;
  stats!: SingleChoiceAnswerStatistics;
  barConfig: ChartConfig | null = null;
  pieConfig: ChartConfig | null = null;

  constructor(private colorService: ColorService) {}

  ngOnInit() {
    this.setUp();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.setUp();
  }

  setUp() {
    this.stats = this.oStats as SingleChoiceAnswerStatistics;
    const colors = this.colorService.assignColors(this.stats.chosenOptionsStats.length);
    this.barConfig = this.prepareBarChartConfig(this.prepareBarChartData(colors));
    this.pieConfig = this.preparePieChartConfig(this.preparePieChartData(colors));
  }

  prepareBarChartConfig(preparedData: Dataset): ChartConfig {
    return {
      type: 'bar',
      labels: preparedData.labels,
      dataset: preparedData.dataset,
      options: {responsive: true},
      legend: false,
    };
  }

  preparePieChartConfig(preparedData: Dataset): ChartConfig {
    return {
      type: 'pie',
      labels: preparedData.labels,
      dataset: preparedData.dataset,
      options: {responsive: true},
      legend: true,
    };
  }

  prepareBarChartData(colors: string[]): Dataset{
    const dataset = {
      data: [] as number[],
      label: 'Number of answers',
      backgroundColor: colors,
    };
    const labels: string[] = [];
    for (let coStat of this.stats.chosenOptionsStats) {
      dataset.data.push(coStat.count);
      labels.push(coStat.value);
    }

    return {
      dataset: [dataset],
      labels: labels,
    }
  }

  preparePieChartData(colors: string[]): Dataset {
    const dataset = {
      data: [] as number [],
      label: 'Answer %',
      backgroundColor: colors
    };
    const labels: string[] = [];
    const sum = this.stats.chosenOptionsStats.reduce((total, co) => {
      return total += co.count;
    }, 0);

    for (let coStat of this.stats.chosenOptionsStats) {
      dataset.data.push(parseFloat((coStat.count * 100 / sum).toFixed(2)));
      labels.push(coStat.value + '%');
    }

    return {
      dataset: [dataset],
      labels: labels,
    }
  }

}


