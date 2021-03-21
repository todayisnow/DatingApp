import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PaginationParams } from '../_models/paginationParams';
import { getPaginationHeaders, getPaginatedResult } from "./paginationHelper";

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  paginationParams: PaginationParams;
  constructor(private http: HttpClient) {
    this.paginationParams = new PaginationParams();
    this.paginationParams.pageSize = 5;
  }

  getUsersWithRoles(paginationParams: PaginationParams) {
   
    this.paginationParams = paginationParams;
    let params = getPaginationHeaders(paginationParams.pageNumber, paginationParams.pageSize);


    return getPaginatedResult<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles', params, this.http);
   
  }
  getPaginationParams() {
    return this.paginationParams;
  }
  updateUserRoles(username: string, roles: string[]) {
    
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
}
