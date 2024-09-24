import { Component, OnInit } from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {SurveyResultStatistics} from "../../models/stats";

@Component({
  selector: 'stats-page',
  templateUrl: 'stats-page.component.html',
  styleUrl: 'stats-page.component.css'
})
export class StatsPageComponent implements OnInit{
  constructor(private activatedRoute: ActivatedRoute, private router: Router) {}

  stats!: SurveyResultStatistics;
  index: number = 0;

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({stats}) => {
      this.stats = stats;
    });

    this.stats.answerStats.sort((a,b) => a.number - b.number);
  }

  goBack(): void {
    this.router.navigate(['/main']);
  }

  protected readonly Math = Math;
  protected readonly console = console;
}
