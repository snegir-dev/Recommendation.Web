export interface ReviewModel {
  reviewId: string;
  author: string;
  urlImages?: string[];
  nameReview: string;
  nameDescription: string;
  description: string;
  authorGrade: number;
  category: string;
  averageCompositionRate: number;
  ownSetRating: number;
  countLike: number;
  isLike: boolean;
  countLikeAuthor: number;
  relatedReviewIds: string[];
  dateCreation: Date;
  tags: Array<string>
}
