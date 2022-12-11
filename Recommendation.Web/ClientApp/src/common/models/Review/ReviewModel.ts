export interface ReviewModel {
  reviewId: string;
  author: string;
  urlImage: string;
  nameReview: string;
  nameDescription: string;
  description: string;
  authorGrade: number;
  category: string;
  averageCompositionRate: number;
  ownSetRating: number;
  isLike: boolean;
  tags: Array<string>
}
