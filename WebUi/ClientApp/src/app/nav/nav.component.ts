import { Component, OnInit } from '@angular/core';
import { AccountService } from "../_services/account.service";
import { Router } from "@angular/router";
import { ToastrService} from 'ngx-toastr'

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
/** nav component*/
export class NavComponent implements OnInit {
  /** nav ctor */
  model: any = {}

  constructor(public accountService: AccountService, private router:Router,private toastr:ToastrService) {

    }
    ngOnInit(): void {
    }
  login() {
    
    this.accountService.login(this.model).subscribe(res => {
      this.router.navigateByUrl('/members');
    }, error => {
        console.error(error);
        this.toastr.error(error.error);
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');


  }
  
}
