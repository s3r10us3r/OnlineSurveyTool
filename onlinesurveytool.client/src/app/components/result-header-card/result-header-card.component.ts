import { Component, Input } from "@angular/core";
import {SurveyHeader} from "../../services/survey.service";



@Component({
  selector: 'result-header-card',
  templateUrl: 'result-header-card.component.html',
  styleUrl: 'result-header-card.component.css',
})
export class ResultHeaderCardComponent{
  @Input() header!: SurveyHeader;
}
