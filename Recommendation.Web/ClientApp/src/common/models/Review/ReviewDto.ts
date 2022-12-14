export interface ReviewDto {
  reviewId: string;
  urlImage: string;
  nameReview: string;
  nameDescription: string;
  averageCompositionRate: number;
  category: string;
  tags: Array<string>;
}
