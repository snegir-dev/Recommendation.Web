import {Component, Input, OnInit} from "@angular/core";
import {CommentService} from "../../common/services/fetches/comment.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CommentModel} from "../../common/models/comment/comment.model";
import {ActivatedRoute} from "@angular/router";
import {SignalrCommentService} from "../../common/services/hubs/signalr.comment.service";

@Component({
  selector: 'app-review-comments',
  templateUrl: './review-comments.component.html',
  styleUrls: ['./review-comments.component.sass']
})
export class ReviewCommentsComponent implements OnInit {
  constructor(private commentService: CommentService,
              public signalrCommentService: SignalrCommentService,
              private route: ActivatedRoute) {

  }

  reviewId!: string;
  commentForm: FormGroup<{ comment: FormControl, reviewId: FormControl }>
    = new FormGroup({
    comment: new FormControl('', [
      Validators.required
    ]),
    reviewId: new FormControl(this.reviewId)
  });

  async ngOnInit() {
    this.fetchUrlParams();
    this.fetchComments();
    await this.signalrCommentService.startConnection(this.reviewId);
    await this.signalrCommentService.addTransferCommentDataListener();
  }

  fetchComments(): void {
    this.commentService.getAll(this.reviewId).subscribe({
      next: value => this.signalrCommentService.comments = value
    });
  }

  fetchUrlParams() {
    this.route.params.subscribe(params => {
      this.reviewId = params['id'];
    });
  }

  createComment() {
    this.commentForm.patchValue({reviewId: this.reviewId});
    this.commentService.post(this.commentForm).subscribe({
      next: async commentId => {
        await this.signalrCommentService.invokeSendComment(this.reviewId, commentId);
      }
    });
  }
}
