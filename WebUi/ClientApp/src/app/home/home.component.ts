import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
/** home component*/
export class HomeComponent implements OnInit {
  /** home ctor */
  registerMode = false;
  constructor(private accountService: AccountService) {

  }
  ngOnInit(): void {
   
    }
  registerToggle() {
    this.registerMode = !this.registerMode;
  }

}
