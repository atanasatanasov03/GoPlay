import { Component, Input, OnInit, SimpleChange } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'app/services/local-storage.service';
import { Post } from '../models/Post';
import { Reported } from '../models/Reported';
import { MessageServiceService } from '../services/message-service.service';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @Input() currentPage = 1;
  post: Post;
  createPost = false;
  report = false;
  homeOrAdmin = true;
  muteUser = false;
  model:any = { }

  constructor(public userService: UserServiceService,
    public localStorage: LocalStorageService,
    public postService: PostsService,
    private router: Router,
    private messageService: MessageServiceService) { this.model.pageSize = 4;}

  ngOnInit(): void {
    if (this.localStorage.getUser() == null) {
      this.router.navigate(['']);
    }
    if (!this.postService.loadedHome)
      this.postService.getPosts(this.currentPage, this.model.pageSize);
    if (this.localStorage.getUser().role == "admin") {
      this.postService.getReportedPosts();
      this.model.period = 0;
      this.model.ban = false;
    }
  }

  changePage() {
    this.postService.getPosts(this.currentPage, this.model.pageSize);

    window.scroll({ 
      top: 0, 
      left: 0, 
      behavior: 'smooth' 
    });
  }

  setPageSize() {
    if(this.model.pageSize > 10) this.model.pageSize = 10;

    this.postService.pagination.pageSize = this.model.pageSize;
    this.postService.pagination.currentPage = 1;
    
    this.currentPage = 1;
    this.postService.getPosts(this.currentPage, this.model.pageSize)
  }

  sendToGroup(post: Post) {
    this.messageService.groupName = post.groupName;
    this.messageService.joinGroup();
    this.router.navigate(["/messages"])
  }

  cancelCreatePost(event: boolean) {
    this.createPost = event;
  }

  reportPost(playPost: Post) {
    this.post = playPost;
    this.report = true;
  }

  innocent(report: Reported) {
    report.toBeRemoved = false;
    this.postService.resolveReport(report);
  }

  mute(report: Reported) {
    this.userService.muteUser(report.reportedPost.userName, this.model.period);
    report.toBeRemoved = true;
    this.postService.resolveReport(report);
  }

  ban(report: Reported) {
    this.userService.banUser(report.reportedPost.userName);
    report.toBeRemoved = true;
    this.postService.resolveReport(report);
  }
}
