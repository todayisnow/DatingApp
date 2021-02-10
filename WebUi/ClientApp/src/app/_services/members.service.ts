import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of } from "rxjs";
import { map } from "rxjs/internal/operators/map";


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
  members:Member[]=[]
  constructor(private http: HttpClient) { }
  getMembers() {
    if (this.members.length > 0) return of(this.members);// as observable
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        for (var m of this.members) {
          m.photoUrl += '?' + Math.random();
          for (var item of m.photos) {
            item.url += "?" + Math.random();
          }
        }
        return members;
      })
      );
  }
  getMember(username: string) {
    const member = this.members.find(x => x.username === username);
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username).pipe(
      map((m:Member) => {
        m.photoUrl += '?'+Math.random();
        for (var item of m.photos) {
          item.url += "?"+Math.random();
        }
        return m;
      })
    );
  }
  updateMember(member: Member) {

    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}
