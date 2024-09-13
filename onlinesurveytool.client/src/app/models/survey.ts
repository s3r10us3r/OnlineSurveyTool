import {Question} from "./question";

export interface Survey {
  name: string,
  questions: Question[],
  id?: string
}
