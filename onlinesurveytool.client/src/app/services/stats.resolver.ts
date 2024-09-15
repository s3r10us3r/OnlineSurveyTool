import {ActivatedRouteSnapshot, ResolveFn, Router } from "@angular/router";
import {SurveyResultStatistics} from "../models/stats";
import {StatsService} from "./stats.service";
import { inject } from "@angular/core";

export const statsResolver: ResolveFn<SurveyResultStatistics> = (route: ActivatedRouteSnapshot) => {
  const surveyId = route.queryParamMap.get('id');
  const statsService = inject(StatsService);
  const router = inject(Router);

  if (surveyId === null)
    router.navigate(['/error']);

  return statsService.getStats(surveyId!);
}
