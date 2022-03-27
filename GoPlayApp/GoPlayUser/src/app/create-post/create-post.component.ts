import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { PostsService } from '../services/posts.service';
import { UserServiceService } from '../services/user.service';
import { NotificationService } from '../services/notification.service';
import { Observable } from 'rxjs';
import { AngularFireStorage } from '@angular/fire/compat/storage';
import { finalize } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  playPost: boolean = true;

  createForm: FormGroup;
  formSubmitted = false;

  selectedFile: File = null;
  fb: any;
  downloadUrl: Observable<string>;

  constructor(public userService: UserServiceService,
    private postService: PostsService,
    private builder: FormBuilder,
    private notificationService: NotificationService,
    private storage: AngularFireStorage
  ) { }

  ngOnInit(): void {
    this.createForm = this.builder.group({
      heading: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
      content: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(60)]],
      address: [''],
      picture: ['']
    })
    this.toggle();
  }

  onFileSelected(event: any) {
    var n = Date.now();

    const file = event.target.files[0];
    const filePath = `RoomsImages/${n}`;
    const fileRef = this.storage.ref(filePath);
    const task = this.storage.upload(`RoomsImages/${n}`, file);

    task.snapshotChanges()
        .pipe(finalize(() => {
            this.downloadUrl = fileRef.getDownloadURL();
            this.downloadUrl.subscribe(url => {
              if (url) this.fb = url;
            });
          })).subscribe();
  }

  createPost() {
    this.formSubmitted = true;
    if(this.createForm.valid) {
      this.postService.createPost(this.buildModel()).subscribe(response => {
        this.cancel();
        this.notificationService.showSuccess("You have succesfully created your post", "")
      }, error => {
        console.log(error);
      })
    }
  }

  buildModel() {
    return this.playPost ? {
      userName: this.userService.user.userName,
      heading: this.createForm.get("heading").value,
      content: this.createForm.get("content").value,
      play: this.playPost,
      address: this.createForm.get("address").value
    } : {
      userName: this.userService.user.userName,
      heading: this.createForm.get("heading").value,
      content: this.createForm.get("content").value,
      play: this.playPost,
      pictureUrl: this.fb,
    }
  }

  toggle() {
    if(this.playPost == true) {
      this.createForm.controls.address.setValidators([Validators.required, Validators.minLength(10), Validators.maxLength(50)]);
      this.createForm.controls.address.updateValueAndValidity();

      this.createForm.controls.picture.setValidators(null);
      this.createForm.controls.picture.updateValueAndValidity();
    } else {
      this.createForm.controls.address.setValidators(null);
      this.createForm.controls.address.updateValueAndValidity();

      this.createForm.controls.picture.setValidators([Validators.required]);
      this.createForm.controls.picture.updateValueAndValidity();
    }
  }

  cancel() {
    this.cancelCreate.emit(false);
  }
}
