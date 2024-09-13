import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
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

  constructor (private http: HttpClient) {}

  public addSurvey(survey: Survey) {
    return this.http.post(`${API_URL}/Survey/add`, survey);
  }

  public getSurvey(id: string) {
    return this.http.get<Survey>(`${API_URL}/Survey/get/${id}`);
  }

  public deleteSurvey(id: string) {
    return this.http.delete(`${API_URL}/Survey/delete/${id}`);
  }

  public getSurveyHeaders(): Observable<SurveyHeader[]> {
    return this.http.get<SurveyHeader[]>(`${API_URL}/Stats/getSurveyHeaders`);
  }

  public openSurvey(id: string): Observable<any> {
    let params = new HttpParams()
      .set('id', id);

    return this.http.patch(`${API_URL}/Survey/open/${id}`, {});
  }

  public closeSurvey(id: string): Observable<any> {
    let params = new HttpParams()
      .set('id', id);

    return this.http.patch(`${API_URL}/Survey/close/${id}`, {});
  }
}
