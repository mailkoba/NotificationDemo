import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
import { timer } from "rxjs";
import { UrlHelper } from "../utils/url-helper";

@Injectable({
    providedIn: "root"
})
export class AuthorizeService {

    private authenticated: boolean | null = null;

    constructor(private readonly httpService: HttpService) {
        timer(5000)
            .subscribe(() => this.authenticated = null);
    }

    isAuthenticated(): Promise<boolean> {
        return new Promise(resolve => {
            const authenticated = this.authenticated;
            if (authenticated === null) {
                this.httpService.postDetail(UrlHelper.security.checkAuthorization)
                    .then(() => {
                        this.authenticated = true;
                        resolve(true);
                    })
                    .catch(() => {
                        this.authenticated = false;
                        resolve(false);
                    });
            } else {
                resolve(authenticated);
            }
        });
    }
}
