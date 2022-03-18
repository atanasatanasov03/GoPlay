export interface Post {
  postId: string;
  userName: string;
  heading: string;
  content: string;
  timeOfCreation: Date;
  play: boolean;

  address?: string;
  groupName?: string;
  pictureUrl?: string;
}
