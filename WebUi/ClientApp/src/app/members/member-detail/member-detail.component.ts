import { Component, OnInit, ViewChild} from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { MembersService } from '../../_services/members.service'
import { Member } from "../../_models/member";
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ToastrService } from "ngx-toastr";
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs: TabsetComponent;
  activeTab: TabDirective;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  messages: Message[] = [];


  constructor(private memberService: MembersService,
    private route: ActivatedRoute,
    private messageService: MessageService,
  private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadMember();
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
  loadMember() {
    
    this.memberService.getMember(this.route.snapshot.paramMap.get('username'))
      .subscribe(member => {
        this.member = member;
        this.galleryImages = this.getImages();
        
      });
   
  }
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading == 'Messages' && this.messages.length===0) {
      this.loadMessages();
    }
  }
  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(message => {
      this.messages = message
    })
  }
}
