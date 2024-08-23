import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Survey} from "../models/survey";
import {API_URL} from "../constants";
import {AuthService} from "./auth.service";

@Injectable()
export class SurveyService {

  constructor (private http: HttpClient, private authService: AuthService) {}

  public AddSurvey(survey: Survey) {
    const token = this.authService.getAccessToken();
    return this.http.post(`${API_URL}/Survey/add`, survey);
  }
}
