import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PlayPost } from '../models/PlayPost';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postUrl = "https://localhost:7170/posts"
  playPosts: PlayPost[];

  constructor(private http: HttpClient) { }

  createPlayPost(model: any) {
    return this.http.post(this.postUrl + '/createPlay', model).pipe(
      map((response: any) => {})
    );
  }

  createNewsPost(model: any) {
    return this.http.post(this.postUrl + '/createNews', model).pipe(
      map((response: any) => {})
    );
  }

  getAllPlayPosts() : Observable<PlayPost[]> {
    console.log("in post service:")
    return this.http.get<PlayPost[]>(this.postUrl + '/getAllPlay');
  }

  getAllNewsPosts() {
    return this.http.get(this.postUrl + '/getAllNews').pipe(
      map((response: any) => {
        const newsPosts = response;
        return newsPosts;
      })
    );
  }

  getAllPostsFor(user: any) { }

}
