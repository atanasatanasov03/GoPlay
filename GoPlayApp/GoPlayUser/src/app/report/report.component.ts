import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Post } from '../models/Post';
import { NotificationService } from '../services/notification.service';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  @Input() postToReport: Post;
  @Output() cancelEvent = new EventEmitter<boolean>();

  reportForm: FormGroup;
  formSubmitted = false;

  constructor(private postService: PostsService,
    private notificationService: NotificationService,
    private builder: FormBuilder) { }

  ngOnInit() {
    this.reportForm = this.builder.group({
      reason: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]]
    });
  }

  submitReport() {
    this.formSubmitted = true;
    if(this.reportForm.valid) {
      this.postService.reportPost(this.postToReport, this.reportForm.get("reason").value).subscribe();
    this.notificationService.showInfo("You have successfully reported " + this.postToReport.userName + "'s post", "")
    this.cancelReport();
    }
  }

  cancelReport() {
    this.cancelEvent.emit(false);
  }

}
