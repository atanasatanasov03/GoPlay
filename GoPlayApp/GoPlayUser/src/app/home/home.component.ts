import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NewsPost } from '../models/NewsPost';
import { PlayPost } from '../models/PlayPost';
import { User } from '../models/User';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  createpost = false;
  playPosts: PlayPost[];
  newsPosts: NewsPost[];

  constructor(private userService: UserServiceService, private postService: PostsService, private router: Router) { }

  ngOnInit(): void {
    if (this.userService.isLogged() != true) {
      this.router.navigate(['']);
    }
    this.postService.getAllPlayPosts().subscribe(data => {
      this.playPosts = data
    });
    this.postService.getAllNewsPosts().subscribe(data => {
      this.newsPosts = data
    });
  }

  createPostToggle() {
    this.createpost = !this.createpost
  }

  cancelCreatePost(event: boolean) {
    this.createpost = event
  } 
}
