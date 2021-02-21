import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { BusyService } from './busy.service';
import { PaginatedResult } from '../_models/pagination';
import { HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { HttpResponse } from "@angular/common/http";
@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
 

  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private busyService: BusyService) { }



  getMessages(pageNumber, pageSize, container) {
    
    let params =getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  async sendMessage(username: string, content: string) {
    //return this.hubConnection.invoke('SendMessage', { recipientUsername: username, content })
    //  .catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
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