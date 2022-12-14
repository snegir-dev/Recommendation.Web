﻿export interface ReviewDto {
  reviewId: string;
  urlImage: string;
  nameReview: string;
  nameDescription: string;
  averageCompositionRate: number;
  category: string;
  countLike: number;
  tags: Array<string>;
}
