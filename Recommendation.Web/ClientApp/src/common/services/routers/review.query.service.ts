﻿import {Injectable} from "@angular/core";

@Injectable()
export class ReviewQueryService {
  page: number = 1;
  pageSize: number = 9;
  searchText!: string;
  filter: string = 'date';
  tag!: string | null;
}
