import { Post } from "./Post";

export interface Reported {
  reportedPost: Post;
  timestamp: Date;
  reporter: string;
  reason: string;
  toBeRemoved?: boolean;
}
