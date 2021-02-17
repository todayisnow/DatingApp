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
  members: Member[] = [];
  constructor(private http: HttpClient) { }
  getMembers(userParams: UserParams) {
    
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);


    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl+'users',params);


    //only get method gets the body but observe will get the whole response then we get out the body

    // if (this.members.length > 0) return of(this.members);//caching as observable "of"
    //return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
    //  map(members => {
    //   // this.members = members;
    //    for (var m of members) {
    //      if (m.photoUrl!=null)
    //      m.photoUrl += '?' + Math.random();
    //      for (var item of m.photos) {
    //        item.url += "?" + Math.random();
    //      }
    //    }
    //    return members;
    //  })
    //  );
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
  getMember(username: string) {
    const member = this.members.find(x => x.username === username);
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username).pipe(
      map((m: Member) => {
        if (m.photoUrl != null)
        m.photoUrl += '?'+Math.random();
        for (var item of m.photos) {
          item.url += "?"+Math.random();
        }
        return m;
      })
    );
  }
  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {})
  }
  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
  updateMember(member: IMember1) {

    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}
