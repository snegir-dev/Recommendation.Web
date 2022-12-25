export interface ReviewDisplayDto {
  reviewId: string;
  urlImage: string;
  nameReview: string;
  nameDescription: string;
  averageCompositionRate: number;
  category: string;
  countLike: number;
  dateCreation: Date;
  tags: Array<string>;
}
