import {Injectable} from "@angular/core";

@Injectable()
export class NumberService {
  isValidNumber(str: string): boolean {
    const parsedVal = parseInt(str, 10);
    return !isNaN(parsedVal) && parsedVal.toString() === str;
  }
}
