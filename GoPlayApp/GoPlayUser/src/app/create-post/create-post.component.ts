import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';
import { NotificationService } from '../services/notification.service';
import { Observable, Subject } from 'rxjs';
import { AngularFireStorage } from '@angular/fire/compat/storage';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  model: any = {}
  playPost: boolean = true;

  constructor(public userService: UserServiceService,
    private postService: PostsService,
    private router: Router,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
  }

  createPost() {
    this.model.userName = this.userService.username;
    this.model.play = this.playPost;
    if (this.playPost) {
      this.postService.createPost(this.model).subscribe(response => {
        console.log(response);
        this.cancel();
        this.notificationService.showSuccess("You have succesfully created your post", "")
        this.router.navigate(['/home'])
      }, error => {
        console.log(error);
      })
    }
    else {
      //this.model.pictureUrl = this.photoUrl;
      console.log(this.model);
    }
  }

  cancel() {
    this.cancelCreate.emit(false);
  }
}
