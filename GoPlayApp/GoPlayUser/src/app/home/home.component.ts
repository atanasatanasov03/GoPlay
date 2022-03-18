import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NewsPost } from '../models/NewsPost';
import { PlayPost } from '../models/PlayPost';
import { MessageServiceService } from '../services/message-service.service';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  post: PlayPost;
  createpost = false;
  report = false;
  loaded = false;
  adminhome = true;

  constructor(public userService: UserServiceService,
    public postService: PostsService,
    private router: Router,
    private messageService: MessageServiceService) { }

  ngOnInit(): void {
    if (this.userService.isLogged() != true) {
      this.router.navigate(['']);
    }
    this.postService.getAllPlayPosts();
    this.postService.getAllNewsPosts();
    if (this.userService.role == "admin")
      this.postService.getReportedPosts();
    this.loaded = true;
  }

  sendToGroup(post: PlayPost) {
    this.messageService.groupName = post.groupName;
    this.router.navigate(["/messages"])
  }

  cancelCreatePost(event: boolean) {
    this.createpost = event
  }

  reportPost(playPost: PlayPost) {
    this.post = playPost;
    this.report = true;
  }
}
