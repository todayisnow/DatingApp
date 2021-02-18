import { Component, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService} from '../../_services/members.service'
import { AccountService } from "../../_services/account.service";
import { Observable } from "rxjs";
import { PaginatedResult, Pagination } from "../../_models/Pagination"
import { UserParams } from "../../_models/userParams"
import { User } from "../../_models/user";



@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParams: UserParams;
  genderList = [{ value: 'male', display: "Males" }, { value: 'female', display: "Females" }]

  constructor(private memberService: MembersService) {
    this.userParams = memberService.getUserParams();
    console.log(this.userParams);
  }

  ngOnInit(): void {
    this.loadMembers();
  
  }
  loadMembers(resetPage?: boolean) {
    if (resetPage != null && resetPage != undefined && resetPage) {
      this.userParams.pageNumber = 1;
    }
    
    this.memberService.getMembers(this.userParams).subscribe((response: PaginatedResult<Member[]>) => {
      this.members = response.result;
      this.pagination = response.pagination;
     
        }
    );
  }
  pageChanged(event: any): void {
    this.userParams.pageNumber = event.page;
   
    this.loadMembers();

  }
  resetFilters() {

    this.userParams = this.memberService.resetUserParams();
    
    this.loadMembers();
  }

}
