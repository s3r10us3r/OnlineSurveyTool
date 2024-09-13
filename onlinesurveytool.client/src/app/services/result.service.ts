import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {Result} from "../models/result";
import {API_URL} from "../constants";
import { Observable } from "rxjs";

@Injectable()
export class ResultService {
  constructor(private http: HttpClient) {}

  public submitResult(result: Result): Observable<any> {
    return this.http.post(`${API_URL}/Answer/answer`, result);
  }
}
