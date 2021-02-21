import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';

import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  pagination: Pagination;
  container = 'Inbox';
  pageNumber = 1;
  pageSize = 5;
  f =1;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  console.log(11);
  }

  loadMessages(t?:number) {
    
    this.f = 2;

    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;


   
      this.f = 1;
    });
   
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1)
    });
    //this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
    //  if (result) {
    //    this.messageService.deleteMessage(id).subscribe(() => {
    //      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
    //    })
    //  }
    //})

  }

  pageChanged(event: any) {
    
    this.pageNumber = event.page;
   
    this.loadMessages();
  }

}
