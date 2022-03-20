import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  post: Post;
  createPost = false;
  report = false;
  homeOrAdmin = true;
  muteUser = false;
  model:any = {}

  constructor(public userService: UserServiceService,
    public postService: PostsService,
    private router: Router,
    private messageService: MessageServiceService) { }

  ngOnInit(): void {
    if (this.userService.isLogged() != true) {
      this.router.navigate(['']);
    }
    if (!this.postService.loadedHome)
      this.postService.getAllPosts();
    if (this.userService.role == "admin") {
      this.postService.getReportedPosts();
      this.model.period = 0;
      this.model.ban = false;
    }
  }

  sendToGroup(post: Post) {
    this.messageService.groupName = post.groupName;
    this.messageService.joinGroup();
    this.router.navigate(["/messages"])
  }

  cancelCreatePost(event: boolean) {
    this.createPost = event
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
