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

  constructor(public accountService: AccountService) {

    }
    ngOnInit(): void {
    }
  login() {
    this.accountService.login(this.model).subscribe(res => { }, error => { console.error(error); });
  }
  logout() {
    this.accountService.logout();


  }
  
}
