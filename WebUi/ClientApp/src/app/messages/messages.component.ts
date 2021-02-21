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
  loadin= false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  console.log(11);
  }

  loadMessages(t?:number) {
    
    this.loadin = true;

    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
     
      this.loadin = false;
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
