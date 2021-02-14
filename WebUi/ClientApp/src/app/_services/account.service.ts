import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { User } from '../_models/user';
import { ReplaySubject } from "rxjs";



import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1); //It buffers a set number of values and will emit those values immediately to any new subscribers in addition to emitting new values to existing subscribers.
  currentUser$ = this.currentUserSource.asObservable();//create observable to subscribe to
  constructor(private http:HttpClient) {

  }
  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {//manipulate the response before subscription 
        const user = response;
        if (user) {
          this.setCurrentUser(user);
          
        }
      })
      );
  }
  register(model: any) {
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }
  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

  }
}

