import {Component} from "@angular/core";
import {Router} from "@angular/router";

@Component({
  selector: 'main-page-component',
  templateUrl: 'main.page.component.html',
  styleUrls: ['../../styles/userForm.css', 'mainpage.component.css']
})
export class MainPageComponent {
  constructor(private router: Router) { }

  handleAddSurvey() {
    this.router.navigate(['/newsurvey'])
  }
}
