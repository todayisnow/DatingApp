import { Component, OnInit, ViewChild} from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { MembersService } from '../../_services/members.service'
import { Member } from "../../_models/member";
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ToastrService } from "ngx-toastr";
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { PresenceService } from '../../_services/presence.service';
@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static:true}) memberTabs: TabsetComponent;//static to fast detect chnages before change detection runs
  activeTab: TabDirective;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  messages: Message[] = [];


  constructor(private memberService: MembersService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    public presence: PresenceService,
  private toastr: ToastrService) { }

  ngOnInit(): void {
   // this.loadMember();
   this.route.data.subscribe(data => {
     this.member = data.member
   });
   this.route.queryParams.subscribe(params => {

     params['tab'] ? this.selectTab(params['tab']) : this.selectTab(0);
   });
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];
    this.galleryImages = this.getImages();
   
    

    
  }

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe(() => {
      this.toastr.success('You have liked ' + member.knownAs);
    });
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url ,
        medium: photo?.url ,
        big: photo?.url 
      });
    }
    return imageUrls;
  }

  onTabActivated(data: TabDirective) {
    
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length===0) {
      this.loadMessages();
    }
  }
  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(message => {
      this.messages = message;
    })
  }
}
