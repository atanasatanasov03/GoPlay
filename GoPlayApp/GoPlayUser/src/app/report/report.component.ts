import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PlayPost } from '../models/PlayPost';
import { NotificationService } from '../services/notification.service';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  @Input() postToReport: PlayPost;
  @Output() cancelEvent = new EventEmitter<boolean>();
  model: any = {};

  constructor(private postService: PostsService,
              private notificationService: NotificationService) { }

  ngOnInit(): void {
  }

  submitReport() {
    this.postService.reportPost(this.postToReport, this.model.reason).subscribe();
    this.notificationService.showInfo("You have successfully reported" + this.postToReport.userName + "'s post", "")
    this.cancelReport();
  }

  cancelReport() {
    this.cancelEvent.emit(false);
  }

}
