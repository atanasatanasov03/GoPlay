import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MessageServiceService } from '../services/message-service.service';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';
import { PlayPost } from '../models/PlayPost'

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  model: any = {}

  constructor(private userService: UserServiceService,
    private postService: PostsService,
    private router: Router,
    private messageService: MessageServiceService
  ) { }

  ngOnInit(): void {
  }

  createPost() {
    console.log(this.userService.username);
    this.model.userName = this.userService.username;
    this.postService.createPlayPost(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
      this.router.navigate(['/home'])
    }, error => {
      console.log(error);
    })

  }

  cancel() {
    this.cancelCreate.emit(false);
  }
}
