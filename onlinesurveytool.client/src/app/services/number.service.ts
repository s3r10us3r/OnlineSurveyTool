import {Injectable} from "@angular/core";

@Injectable()
export class NumberService {
  isValidNumber(str: string): boolean {
    const parsedVal = parseInt(str, 10);
    return !isNaN(parsedVal) && parsedVal.toString() === str;
  }

  isValidDouble(str: string): boolean {
    const num = Number(str);
    return !Number.isNaN(num) && num !== -0;
  }

  isValidInteger(str: string): boolean {
    const numPars = Number(str);
    const intPars = parseInt(str);

    return numPars === intPars && numPars !== -0;
  }
}
