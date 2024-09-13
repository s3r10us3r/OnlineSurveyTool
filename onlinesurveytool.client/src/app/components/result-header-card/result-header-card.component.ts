import { Component, EventEmitter, Input, Output, ViewEncapsulation } from "@angular/core";
import {SurveyHeader, SurveyService} from "../../services/survey.service";
import { Router } from "@angular/router";

@Component({
  selector: 'result-header-card',
  templateUrl: 'result-header-card.component.html',
  styleUrl: 'result-header-card.component.css',
  encapsulation: ViewEncapsulation.None
})
export class ResultHeaderCardComponent{
  @Input() header!: SurveyHeader;
  @Output() updateEvent = new EventEmitter<[string, string]>();

  constructor(private surveyService: SurveyService, private router: Router) {}

  openClose(): void {
    if (this.header.isOpen)
      this.close();
    else
      this.open();
  }

  open(): void {
    const obs = this.surveyService.openSurvey(this.header.id);
    obs.subscribe({
      next: () => this.handleOpening(),
      error: err => this.handleError(err)
    })
  }

  handleOpening(): void {
    this.header.isOpen = true;
    this.updateEvent.emit(['update', this.header.id]);
  }

  handleError(err: any): void {
    console.error(err);
    this.router.navigate(['/error']);
  }

  close(): void {
    const obs = this.surveyService.closeSurvey(this.header.id);
    obs.subscribe({
      next: () => this.handleClosing(),
      error: err => this.handleError(err)
    })
  }

  handleClosing(): void {
    this.header.isOpen = false;
    this.updateEvent.emit(['update', this.header.id]);
  }

  getRootStyle(): object {
    return {
      'border-color': this.header.isOpen ? 'green' : 'red'
    }
  }

  getLinkToSurvey(): void {
    if (!this.header.isOpen)
      return;

    const text = this.generateLink();

    navigator.clipboard.writeText(text).then(
      () => {alert('Link to survey copied to clipboard!')}
    ).catch(
      err => console.error(err)
    )
  }

  generateLink(): string {
    const baseUrl = window.location.protocol + '//' + window.location.host;
    return baseUrl + `/survey/${this.header.id}`
  }

  editSurvey(): void {

  }

  deleteSurvey(): void {
    this.surveyService.deleteSurvey(this.header.id).subscribe({
      next: res => {this.updateEvent.emit(['delete', this.header.id])},
      error: err => {console.error(err); this.router.navigate(['/error'])}
    })
  }

  protected readonly navigator = navigator;
}
