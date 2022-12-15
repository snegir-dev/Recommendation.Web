import {Injectable} from "@angular/core";
import {ActivatedRoute} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  constructor(private route: ActivatedRoute) {
  }

  getValueFromQueryParams<T>(queryParam: string): T {
    let value: T | null = null;
    this.route.params.subscribe(params => {
      value = params[queryParam];
    });

    if (!value)
      throw new Error(`Parameter '${queryParam}' not found`);

    return value;
  }
}
