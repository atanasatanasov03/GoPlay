import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NewsPost } from '../models/NewsPost';
import { PlayPost } from '../models/PlayPost';
import { ReportedPost } from '../models/ReportedPost';
import { UserServiceService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postUrl = "https://localhost:7170/posts"
  public PlayPosts: PlayPost[] = [];
  public NewsPosts: NewsPost[] = [];
  public ReportedPosts: PlayPost[] = [];
  public AllPosts: (PlayPost | NewsPost)[] = [];

  constructor(private http: HttpClient, private userService: UserServiceService) { }

  createPlayPost(model: any) {
    return this.http.post<PlayPost>(this.postUrl + '/createPlay', model).pipe(
      map((response: PlayPost) => { this.PlayPosts.push(response) })
    );
   }

  createNewsPost(model: any) {
    return this.http.post<NewsPost>(this.postUrl + '/createNews', model).pipe(
      map((response: NewsPost) => { this.NewsPosts.push(response) })
    );
  }

  getAllPlayPosts() {
    this.http.get<PlayPost[]>(this.postUrl + '/getAllPlay').subscribe(res => this.PlayPosts = res);
  }

  getAllNewsPosts() {
    this.http.get<NewsPost[]>(this.postUrl + '/getAllNews').subscribe(res => this.NewsPosts = res);
  }

  getReportedPosts() {
    this.http.get<PlayPost[]>(this.postUrl + '/reported').subscribe(res => this.ReportedPosts = res);
  }

  reportPost(post: PlayPost, reason: string) {
    return this.http.post(this.postUrl + '/report', this.buildReport(post, reason));
  }

  buildReport(playPost: PlayPost, reason: string): ReportedPost {
    return {
      postId: playPost.id,
      username: this.userService.username,
      reason: reason
    }
  }

  getPosts() {
    for (let i = 0, ppi = 0, npi = 0; i < (this.PlayPosts.length + this.NewsPosts.length); i++) {
      if (i % 5 != 0) {
        this.AllPosts.push(this.PlayPosts[ppi]);
        ppi++;
      }
      else {
        this.AllPosts.push(this.NewsPosts[npi]);
        npi++;
      }
    }
  }
}
