import {Component, Input, OnInit} from "@angular/core";
import {CommentService} from "../../common/services/fetches/comment.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CommentModel} from "../../common/models/comment/comment.model";
import {ActivatedRoute} from "@angular/router";

@Component({
    selector: 'app-review-comments',
    templateUrl: './review-comments.component.html',
    styleUrls: ['./review-comments.component.sass']
})
export class ReviewCommentsComponent implements OnInit {
    constructor(private commentService: CommentService,
                private route: ActivatedRoute) {
    }

    reviewId!: string;
    comments!: CommentModel[];
    commentForm: FormGroup<{ comment: FormControl, reviewId: FormControl }>
        = new FormGroup({
        comment: new FormControl('', [
            Validators.required
        ]),
        reviewId: new FormControl(this.reviewId)
    });

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.reviewId = params['id'];
        });
        this.fetchComments();
    }

    fetchComments(): void {
        this.commentService.getAll(this.reviewId).subscribe({
            next: value => this.comments = value
        });
    }

    createComment() {
        this.commentForm.patchValue({
            reviewId: this.reviewId
        });
        this.commentService.post(this.commentForm).subscribe({
            next: _ => this.fetchComments()
        });
    }
}
