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
  countLike: number;
  isLike: boolean;
  countLikeAuthor: number;
  dateCreation: Date;
  tags: Array<string>
}
