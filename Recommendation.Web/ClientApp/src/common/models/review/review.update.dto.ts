export interface ReviewUpdateDto {
  urlImage: string;
  reviewId: string;
  nameReview: string;
  nameDescription: string;
  description: string;
  authorGrade: number;
  category: string;
  tags: string[];
}
