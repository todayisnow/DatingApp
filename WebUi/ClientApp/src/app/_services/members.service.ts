import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member, Member as IMember, Member as IMember1 } from '../_models/member';
import { of } from "rxjs";
import { map } from "rxjs/internal/operators/map";
import { PaginatedResult } from '../_models/Pagination';
import { HttpParams } from "@angular/common/http";
import { UserParams} from '../_models/userParams'
import { HttpResponse } from "@angular/common/http";
import { AccountService } from "./account.service";
import { User } from "../_models/user";
import { take } from "rxjs/operators";
// interceptor used instead 
//const httpOptions = {
//  headers: new HttpHeaders({
//    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
//  })
//}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  memberCache= new Map();
  userParams: UserParams;
  user: User;

  constructor(private http: HttpClient, private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user: User) => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  getUserParams() {
    return this.userParams;
  }
  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  private setUserParams(userParams: UserParams) {
    this.userParams = userParams;
  }

  getMembers(userParams: UserParams) {
    this.setUserParams(userParams);
    let currentKey = Object.values(userParams).join('-');
    var resposne = this.memberCache.get(currentKey);
    if (resposne) return of(resposne);//caching as observable "of"



    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);


    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(members => {
        this.memberCache.set(currentKey, members);
        return members;
      })
      );
  }


 
  getMember(username: string) {

    
    const member = [...this.memberCache.values()]//spread operator
      .reduce((arr, elem) => arr.concat(elem.result), [])//call back for all the elems 
      .find((member: Member) => member.username === username);

    if (member) return of(member);
   return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {})
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  updateMember(member: IMember1) {

    return this.http.put(this.baseUrl + 'users', member);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {})
  }

  getLikes(predicate: string, pageNumber, pageSize) {
    let params = this.getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);
  }

  private getPaginatedResult<T>(url, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map((response: HttpResponse<T>) => {

        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      }));
  }
  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();//serialize parameters and adding to query string

    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;


  }
}
