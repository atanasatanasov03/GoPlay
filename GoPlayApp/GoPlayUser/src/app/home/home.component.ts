import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PlayPost } from '../models/PlayPost';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  createpost = false;
  posts : PlayPost[];

  constructor(private userService: UserServiceService, private postService: PostsService, private router: Router) { }

  ngOnInit(): void {
    if (this.userService.isLogged() != true) {
      this.router.navigate(['']);
    }
    this.postService.getAllPlayPosts().subscribe(data => {
      console.log(data)
      this.posts = data
    });
  }

  createPostToggle() {
    this.createpost = !this.createpost
  }

  cancelCreatePost(event: boolean) {
    this.createpost = event
  } 
}
