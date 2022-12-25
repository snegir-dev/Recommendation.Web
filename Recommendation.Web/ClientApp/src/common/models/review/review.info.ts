import {ReviewCardDto} from "./reviewCardDto";


export interface ReviewInfo {
  totalCountReviews: number,
  reviewDtos: Array<ReviewCardDto>
}
