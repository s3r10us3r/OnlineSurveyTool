import { Component, Input, OnChanges, SimpleChanges} from "@angular/core";
import {SurveyHeader} from "../../services/survey.service";

@Component({
  selector: 'header-list',
  templateUrl: 'header-list.component.html',
  styleUrl: 'header-list.component.css'
})
export class HeaderListComponent implements OnChanges {
  @Input() headers: SurveyHeader[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    this.sortHeaders();
  }

  getUpdate(args: [string, string]) {
    const [eventType, id] = args;
    if (eventType === 'delete') {
      this.headers = this.headers.filter(s => s.id !== id);
    }

    if (eventType === 'update') {
      this.sortHeaders();
    }
  }

  sortHeaders() {
    const openHeaders = this.headers.filter(sh => sh.isOpen);
    const closedHeaders = this.headers.filter(sh => !sh.isOpen);

    this.headers = openHeaders.concat(closedHeaders);
  }
}
