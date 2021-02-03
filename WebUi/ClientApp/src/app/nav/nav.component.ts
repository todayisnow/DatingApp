import { Component, OnInit } from '@angular/core';
import { AccountService } from "../_services/account.service";

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
/** nav component*/
export class NavComponent implements OnInit {
  /** nav ctor */
  model: any = {}
  loggedIn:boolean;
  constructor(private accountService:AccountService) {

    }
    ngOnInit(): void {
       // throw new Error('Method not implemented.');
  }
  login() {
    this.accountService.login(this.model).subscribe(res => {
        console.log(res);
        this.loggedIn = true;
      },
      error => { console.error(error); });
  }
  logout() {
    this.loggedIn = false;

  }
}
