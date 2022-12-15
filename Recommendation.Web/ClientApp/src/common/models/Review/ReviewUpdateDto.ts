import {ReviewFormModel} from "./ReviewFormModel";

export interface ReviewUpdateDto {
  reviewId: string;
  nameReview: string;
  nameDescription: string;
  description: string;
  authorGrade: number;
  category: string;
  tags: string[];
}
