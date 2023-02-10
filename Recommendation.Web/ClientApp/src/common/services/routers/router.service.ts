﻿import {Injectable} from "@angular/core";
import {ActivatedRoute} from "@angular/router";

@Injectable()
export class RouterService {
  constructor(private route: ActivatedRoute) {
  }

  getValueFromParams<T extends number | string>(queryParam: string): T {
    let value: T | undefined | null;
    this.route.params.subscribe(params => {
      value = params[queryParam];
    });

    if (!value)
      value = <T>'';

    return value;
  }

  getValueFromQueryParams<T extends number | string>(queryParam: string): T {
    let value: T | undefined | null;
    this.route.queryParams.subscribe(params => {
      value = params[queryParam];
    });

    if (!value)
      value = <T>'';

    return value;
  }
}
