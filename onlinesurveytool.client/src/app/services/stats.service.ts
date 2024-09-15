import { HttpClient } from "@angular/common/http";
import {SurveyResultStatistics} from "../models/stats";
import { Observable } from "rxjs";
import {API_URL} from "../constants";
import { Injectable } from "@angular/core";

@Injectable()
export class StatsService {
  constructor(private http: HttpClient) {}

  public getStats(id: string): Observable<SurveyResultStatistics> {
    return this.http.post<SurveyResultStatistics>(`${API_URL}/Stats/surveyStats`, {id: id});
  }
}
