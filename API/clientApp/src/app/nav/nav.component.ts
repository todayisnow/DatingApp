import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
/** nav component*/
export class NavComponent implements OnInit {
  /** nav ctor */
  model: any={ }
  constructor() {

    }
    ngOnInit(): void {
       // throw new Error('Method not implemented.');
  }
  login() {
    console.log(this.model)
  }
}
