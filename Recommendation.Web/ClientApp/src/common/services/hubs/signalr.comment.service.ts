import {Injectable} from "@angular/core";
import * as signalR from "@microsoft/signalr"
import {CommentModel} from "../../models/comment/comment.model";

@Injectable({
  providedIn: 'root'
})
export class SignalrCommentService {
  comments: CommentModel[] = [];

  private hubCommentConnection = new signalR.HubConnectionBuilder()
    .withUrl('/comment-hub')
    .build();

  async startConnection(groupName: string) {
    await this.hubCommentConnection.start();
    await this.hubCommentConnection.invoke('ConnectGroup', groupName);
  }

  async invokeSendComment(reviewId: string, commentId: string) {
    await this.hubCommentConnection.invoke("SendComment", reviewId, commentId);
  }

  async addTransferCommentDataListener() {
    await this.hubCommentConnection.on('GetComment', (comment: CommentModel) => {
      this.comments.push(comment);
    });
  }

  async stopConnection() {
    await this.hubCommentConnection.stop();
  }
}
