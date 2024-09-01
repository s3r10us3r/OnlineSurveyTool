import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Survey} from "../models/survey";
import {API_URL} from "../constants";
import {AuthService} from "./auth.service";
import { Observable } from 'rxjs';

export interface SurveyHeader {
  id: string,
  name: string,
  isOpen: boolean,
  resultCount: number
}

@Injectable()
export class SurveyService {

  constructor (private http: HttpClient, private authService: AuthService) {}

  public AddSurvey(survey: Survey) {
    return this.http.post(`${API_URL}/Survey/add`, survey);
  }

  public GetSurveyHeaders(): Observable<SurveyHeader[]> {
    return this.http.get<SurveyHeader[]>(`${API_URL}/Stats/getSurveyHeaders`);
  }
}
