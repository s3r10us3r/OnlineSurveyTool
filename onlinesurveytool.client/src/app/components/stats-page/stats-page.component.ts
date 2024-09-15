import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {SurveyResultStatistics} from "../../models/stats";

@Component({
  selector: 'stats-page',
  templateUrl: 'stats-page.component.html',
  styleUrl: 'stats-page.component.css'
})
export class StatsPageComponent implements OnInit{
  constructor(private activatedRoute: ActivatedRoute) {}

  stats!: SurveyResultStatistics;
  index: number = 0;

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({stats}) => {
      this.stats = stats;
    });

    this.stats.answerStats.sort((a,b) => a.number - b.number);
  }

  protected readonly Math = Math;
  protected readonly console = console;
}
