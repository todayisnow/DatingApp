import { Component, OnInit } from '@angular/core';
import { AccountService } from "../_services/account.service";
import { Router } from "@angular/router";
import { ToastrService} from 'ngx-toastr'
import { MembersService } from "../_services/members.service";

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
/** nav component*/
export class NavComponent implements OnInit {
  /** nav ctor */
  model: any = {}

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService,
    private membersService:MembersService)
   {

    }
    ngOnInit(): void {
    }
  login() {
    
    this.accountService.login(this.model).subscribe(res => this.router.navigateByUrl('/members'));
  }
  logout() {
    this.accountService.logout();
    this.membersService.resetMemberCache();
    this.router.navigateByUrl('/');


  }
  
}
