import {ReviewDto} from "./ReviewDto";

export interface ReviewInfo {
  totalCountReviews: number,
  reviewDtos: Array<ReviewDto>
}
