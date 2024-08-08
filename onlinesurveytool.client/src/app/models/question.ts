import {ChoiceOption} from "./choice.option";

export enum QuestionType {
  SingleChoice = 'Single Choice',
  MultipleChoice = 'Multiple Choice',
  NumericalInteger = 'Numerical Integer',
  NumericalDouble = 'Numerical Double',
  Textual = 'Textual'
}

export interface Question {
    number: number;
    value: string;
    type: QuestionType;
    canBeSkipped: boolean;
    id?: string;
    minimum?: number;
    maximum?: number;
    choiceOptions?: Array<ChoiceOption>
}

export function questionPrototype(type: QuestionType, number: number = 0, value: string = ''): Question{
  switch(type) {
    case QuestionType.SingleChoice:
      return {
        number: number,
        value: value,
        type: QuestionType.SingleChoice,
        canBeSkipped: true,
        choiceOptions: []
      }

    case QuestionType.MultipleChoice:
      return {
        number: number,
        value: value,
        type: QuestionType.SingleChoice,
        canBeSkipped: true,
        choiceOptions: [],
        minimum: 0,
        maximum: 0
    }

    case QuestionType.NumericalDouble:
      return {
        number: number,
        value: value,
        type: QuestionType.NumericalDouble,
        canBeSkipped: true,
        minimum: 0,
        maximum: 100
      }

    case QuestionType.NumericalInteger:
      return {
        number: number,
        value: value,
        type: QuestionType.NumericalInteger,
        canBeSkipped: true,
        minimum: 0,
        maximum: 100
      }

    case QuestionType.Textual:
      return {
        number: number,
        value: value,
        type: QuestionType.Textual,
        canBeSkipped: true,
        minimum: 0,
        maximum: 100
      }
  }
}
