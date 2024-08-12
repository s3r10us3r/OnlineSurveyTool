import {TestBed} from '@angular/core/testing'
import {NumberService} from "./number.service";

describe('NumberService', () => {
  let service: NumberService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NumberService]
    });
    service = TestBed.inject(NumberService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should check if string is a number', () => {
    expect(service.isValidNumber('1267')).toBeTrue();
    expect(service.isValidNumber('123')).toBeTrue();
    expect(service.isValidNumber('astring')).toBeFalse();
    expect(service.isValidNumber('')).toBeFalse();
    expect(service.isValidNumber('    ')).toBeFalse();
    expect(service.isValidNumber('12 12')).toBeFalse();
    expect(service.isValidNumber('12a12')).toBeFalse();
  });
})
