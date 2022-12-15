import {ReviewDisplayDto} from "./ReviewDisplayDto";

export interface ReviewInfo {
  totalCountReviews: number,
  reviewDtos: Array<ReviewDisplayDto>
}
