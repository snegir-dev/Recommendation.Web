import {ImageMetadata} from "../image/image.metadata";

export interface ReviewUpdateDto {
  imageMetadatas?: ImageMetadata[];
  reviewId: string;
  nameReview: string;
  nameDescription: string;
  description: string;
  authorGrade: number;
  category: string;
  tags: string[];
}
