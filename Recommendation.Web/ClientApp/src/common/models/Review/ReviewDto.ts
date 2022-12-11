export interface ReviewDto {
  reviewId: string;
  urlImage: string;
  nameReview: string;
  nameDescription: string;
  averageRate: number;
  category: string;
  tags: Array<string>;
}
