import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MessageServiceService } from '../services/message-service.service';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';
import { PlayPost } from '../models/PlayPost'
import { NotificationService } from '../services/notification.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, takeUntil } from 'rxjs/operators';
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

  path: string;

  constructor(public userService: UserServiceService,
    private postService: PostsService,
    private router: Router,
    private notificationService: NotificationService,
    private storage: AngularFireStorage
  ) { }

  ngOnInit(): void {
  }

  downloadURL: Observable<string>;
  photoUrl: string;

  upload($event : any) {
    var n = Date.now();

    const filePath = "NewsPhotos/" + this.model.heading + "/" + n;
    const fileRef = this.storage.ref(filePath);

    const task = this.storage.upload(filePath, $event.target.files[0]);
    console.log("bob")
    /*task.snapshotChanges()
      .pipe(finalize(() => {
        this.downloadURL = fileRef.getDownloadURL();
        this.downloadURL.subscribe(url => {
          if (url) this.photoUrl = url;
        });
      })
      ).subscribe(url => { if (url) console.log(url); });*/
  }

  createPost() {
    this.model.userName = this.userService.username;
    if (this.playPost) {
      this.postService.createPlayPost(this.model).subscribe(response => {
        console.log(response);
        this.cancel();
        this.notificationService.showSuccess("You have succesfully created your post", "")
        this.router.navigate(['/home'])
      }, error => {
        console.log(error);
      })
    }
    else {
      this.model.pictureUrl = this.photoUrl;
      console.log(this.model);
    }
  }

  cancel() {
    console.log(this.playPost)
    this.cancelCreate.emit(false);
  }
}
