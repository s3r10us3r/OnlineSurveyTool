export interface SurveyResultStatistics {
  surveyName: string,
  resultCount: number,
  answerStats: AnswerStatistics[]
}

export interface AnswerStatistics {
  type: string,
  number: number,
  questionValue: string,
  numberOfAnswers: number
}

export interface SingleChoiceAnswerStatistics extends AnswerStatistics {
  chosenOptionStats: ChoiceOptionStat[]
}

export interface ChoiceOptionStat {
  value: string,
  count: number
}

export interface MultipleChoiceAnswerStatistics extends AnswerStatistics {
  chosenOptionsStats: ChosenOptionsCombinationStat[]
}

export interface ChosenOptionsCombinationStat {
  values: string[],
  count: number
}

export interface NumericalAnswerStatistics extends AnswerStatistics {
  average: number,
  median: number,
  dominant: number,
  minimum: number,
  maximum: number,
  countOnSegment: SegmentStat[]
}

export interface SegmentStat {
  start: number,
  end: number,
  count: number
}

export interface TextualAnswerStatistics extends AnswerStatistics {
  averageLength: number,
  mostCommonWord: { [key: string]: number }
}
