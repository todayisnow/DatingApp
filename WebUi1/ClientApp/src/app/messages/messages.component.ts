import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';

import { MessageService } from '../_services/message.service';
import { finalize} from "rxjs/operators"
import { ConfirmService } from '../_services/confirm.service';
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
  loading= false;

  constructor(private messageService: MessageService, private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.loadMessages();

  }

  loadMessages() {
    
   
this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container)
      //.pipe(finalize(() => { console.log(1); }))
      .subscribe(response => {

        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      });

  }

  deleteMessage(id: number) {
    this.confirmService.confirm('Delete', 'this can not be undone')
      .subscribe(result => {
        if (result)
          this.messageService.deleteMessage(id).subscribe(() => {
            this.messages.splice(this.messages.findIndex(m => m.id === id), 1)
          });
      });
   
    

  }

  pageChanged(event: any) {
   
    this.pageNumber = event.page;
    this.loadMessages();
    ;
  }

}
