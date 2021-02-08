import { Component, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService} from '../../_services/members.service'
import { AccountService } from "../../_services/account.service";
import { Observable } from "rxjs";

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members$: Observable< Member[]>;
  constructor(private memberService: MembersService, public accountService: AccountService) { }

  ngOnInit(): void {
    this.members$ = this.memberService.getMembers();
  
  }

}
