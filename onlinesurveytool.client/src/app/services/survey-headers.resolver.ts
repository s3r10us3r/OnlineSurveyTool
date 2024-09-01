import { ResolveFn } from "@angular/router";
import {SurveyHeader, SurveyService} from "./survey.service";
import { inject } from "@angular/core";
import {AuthService} from "./auth.service";
import {catchError, of, switchMap } from "rxjs";

export const surveyHeadersResolver: ResolveFn<SurveyHeader[]>= (
) => {
  const authService = inject(AuthService);
  const surveyService = inject(SurveyService);

  return authService.isLoggedIn().pipe(
    switchMap(isLoggedIn => {
      if (isLoggedIn)
        return surveyService.GetSurveyHeaders();
      else
        return of([]);
    }),
    catchError(() => {
      return of([]);
    })
  )
};
