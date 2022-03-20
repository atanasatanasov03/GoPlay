import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { PaginationParameters } from '../models/PaginationParameters';
import { Post } from '../models/Post';
import { Report } from '../models/Report';
import { Reported } from '../models/Reported';
import { UserServiceService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  postUrl = "https://localhost:7170/posts"
  public loadedHome: boolean = false;
  public loadedAdmin: boolean = false;
  public Posts: Post[] = [];
  public Reported: Reported[] = [];
  public pagination: PaginationParameters;

  constructor(private http: HttpClient, private userService: UserServiceService) { }

  createPost(model: any) {
    console.log("yo")
    return this.http.post<Post>(this.postUrl + '/createPost', model).pipe(
      map((response: Post) => { this.Posts.push(response) })
    );
   }

  getAllPosts() {
    this.http.get<Post[]>(this.postUrl + '/paged?pageNumber=1&pageSize=3', {observe: 'response'})
      .subscribe(res => {
        this.Posts = res.body;
        this.loadedHome = true;
        this.pagination = JSON.parse(res.headers.get("X-Pagination"));
      });
  }

  getReportedPosts() {
    this.http.get<Reported[]>(this.postUrl + '/reported').subscribe(res => { this.Reported = res; this.loadedAdmin = true; });
  }

  reportPost(post: Post, reason: string) {
    return this.http.post(this.postUrl + '/report', this.buildReport(post, reason));
  }

  buildReport(post: Post, reason: string): Report {
    return {
      postId: post.postId,
      username: this.userService.username,
      reason: reason
    }
  }

  resolveReport(report: Reported) {
    this.Reported.splice(this.Reported.indexOf(report), 1);
    if (report.toBeRemoved) this.Posts.splice(this.Posts.indexOf(report.reportedPost), 1);
    return this.http.post(this.postUrl + '/resolveReport', report).subscribe(_ => console.log("done"));
  }
}
