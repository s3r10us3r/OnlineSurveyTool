import {Component, Input, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import {AnswerStatistics, Dataset, MultipleChoiceAnswerStatistics} from "../../models/stats";
import {ColorService} from "../../services/color.service";
import {Data} from "@angular/router";
import {ChartConfig} from "../question-stats/question-stats.component";

@Component({
  selector: 'multiple-choice-stats',
  templateUrl: 'multiple-choice-stats.component.html',
  styleUrl: 'multiple-choice-stats.component.css'
})
export class MultipleChoiceStatsComponent implements OnInit, OnChanges {
  @Input() oStats!: AnswerStatistics;
  stats!: MultipleChoiceAnswerStatistics;
  barChartConfig!: ChartConfig;

  constructor(private colorService: ColorService) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.setUp();
  }

  ngOnInit(): void {
    this.setUp();
  }

  setUp(): void {
    this.stats = this.oStats as MultipleChoiceAnswerStatistics;
    this.stats.chosenOptionCombinationStats = this.stats.chosenOptionCombinationStats.sort((a, b) => b.count - a.count);
    const colors = this.colorService.assignColors(this.stats.chosenOptionStats.length);
    const data = this.prepareData(colors);
    this.barChartConfig = this.prepareBarChartConfig(data);
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

  prepareData(colors: string[]): Dataset {
    const dataset = {
      data: [] as number[],
      label: 'Number of answers',
      backgroundColor: colors,
    };
    const labels: string[] = [];
    for (let coStat of this.stats.chosenOptionStats) {
      dataset.data.push(coStat.count);
      labels.push(coStat.value);
    }

    return {
      dataset: [dataset],
      labels: labels,
    }
  }

  stringArrayToText(arr: string[]): string {
    let result = '';
    for (let i = 0; i < arr.length; i++) {
      result += arr[i];
      if (i != arr.length - 1)
        result += ', ';
    }
    return result;
  }
}
