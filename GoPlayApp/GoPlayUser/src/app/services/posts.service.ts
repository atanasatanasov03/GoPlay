import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NewsPost } from '../models/NewsPost';
import { PlayPost } from '../models/PlayPost';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postUrl = "https://localhost:7170/posts"
  /*PlayPosts: PlayPost[];
  NewsPosts: NewsPost[];*/

  constructor(private http: HttpClient) { }

  createPlayPost(model: any): Observable<PlayPost> {
    return this.http.post<PlayPost>(this.postUrl + '/createPlay', model).pipe(
      map((response: PlayPost) => { return response; })
    );
  }

  createNewsPost(model: any) {
    return this.http.post(this.postUrl + '/createNews', model).pipe(
      map((response: any) => {})
    );
  }

  getAllPlayPosts() : Observable<PlayPost[]> {
    return this.http.get<PlayPost[]>(this.postUrl + '/getAllPlay');
  }

  getAllNewsPosts() {
    return this.http.get<NewsPost[]>(this.postUrl + '/getAllNews');
  }

  /*getPlayPostsFor(user: any) {
    this.http.get<PlayPost[]>(this.postUrl + '/getPlayPostsFor?username=' + user.username).subscribe(pp => this.PlayPosts = pp);

    return this.PlayPosts;
  }

  getNewsPostsFor(user: any) {
    this.http.get<NewsPost[]>(this.postUrl + '/getNewsPostsFor?username=' + user.username).subscribe(np => this.NewsPosts = np);

    return this.NewsPosts;
  }*/
}
