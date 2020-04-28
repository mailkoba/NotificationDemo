import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, concat } from "rxjs";
import { take, filter, tap } from "rxjs/operators";
import { HttpService } from "./http.service";
import { UrlHelper } from "../utils/url-helper";
import { UserContextDto } from "../models/models";

@Injectable({
    providedIn: "root"
})
export class ContextService {

    constructor(private readonly httpService: HttpService) {
    }

    get context(): Observable<UserContextDto> {
        return concat(
            this.userContext.pipe(
                take(1),
                filter(x => !!x)
            ),
            this.httpService.postObservable<UserContextDto>(UrlHelper.security.getUserContext, null, true)
            .pipe(
                filter(x => !!x),
                tap(x => this.userContext.next(x))
            ),
            this.userContext.asObservable());
    }

    private userContext = new BehaviorSubject<UserContextDto>(null);
}
