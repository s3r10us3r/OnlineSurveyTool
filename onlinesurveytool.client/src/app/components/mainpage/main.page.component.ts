import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {SurveyHeader, SurveyService} from "../../services/survey.service";

@Component({
  selector: 'main-page-component',
  templateUrl: 'main.page.component.html',
  styleUrls: ['../../styles/userForm.css', 'mainpage.component.css']
})
export class MainPageComponent implements OnInit{
  headers: SurveyHeader[] = [];

  constructor(private router: Router, private surveyService: SurveyService) { }

  ngOnInit(): void {
    this.surveyService.getSurveyHeaders().subscribe({
      next: res => this.handleNext(res),
      error: err => this.handleErr(err)
    });
  }

  handleNext(headers: SurveyHeader[]) {
    this.headers = headers;
  }

  handleErr(error: any) {
    console.error('Error when fetching headers.', error);
    this.router.navigate(['/error']);
  }

  handleAddSurvey() {
    this.router.navigate(['/newsurvey'])
  }
}
