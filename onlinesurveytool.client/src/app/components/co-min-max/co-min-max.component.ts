import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";


@Component({
  selector: 'co-min-max',
  templateUrl: 'co-min-max.component.html',
  styleUrl: 'co-min-max.component.css'
})
export class CoMinMaxComponent implements OnInit{
  @Input() min!: number;
  @Input() max!: number;
  @Output() number = new EventEmitter<number>();
  numberValue: number = 0;

  constructor() {}

  ngOnInit() {
     this.numberValue = this.min;
    this.sendData();
  }

  inc() {
    if (this.numberValue < this.max) {
      this.numberValue += 1;
    }
    this.sendData();
  }

  dec() {
    if (this.numberValue > this.min) {
      this.numberValue -= 1;
    }
    this.sendData();
  }

  sendData() {
    this.number.emit(this.numberValue);
  }
}
