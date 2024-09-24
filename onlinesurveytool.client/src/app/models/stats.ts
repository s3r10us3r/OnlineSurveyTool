export interface SurveyResultStatistics {
  surveyName: string,
  resultCount: number,
  answerStats: AnswerStatistics[]
}

export interface AnswerStatistics {
  type: string,
  number: number,
  questionValue: string,
  numberOfAnswers: number,
  isSkippable: boolean
}

export interface SingleChoiceAnswerStatistics extends AnswerStatistics {
  chosenOptionsStats: ChoiceOptionStat[]
}

export interface ChoiceOptionStat {
  value: string,
  count: number
}

export interface MultipleChoiceAnswerStatistics extends AnswerStatistics {
  chosenOptionCombinationStats: ChosenOptionsCombinationStat[],
  chosenOptionStats: ChoiceOptionStat[],
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
  countOnAnswers: AnswerCount[]
}

export interface AnswerCount {
  answer: number,
  count: number
}

export interface TextualAnswerStatistics extends AnswerStatistics {
  averageLength: number,
  mostCommonWords: { [key: string]: number }
}


export type Dataset = {dataset: [{data: number[], label: string, backgroundColor?: string[]}], labels: string[]};
