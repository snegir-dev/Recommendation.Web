import {ReviewDisplayDto} from "./review.display.dto";


export interface ReviewInfo {
  totalCountReviews: number,
  reviewDtos: Array<ReviewDisplayDto>
}
