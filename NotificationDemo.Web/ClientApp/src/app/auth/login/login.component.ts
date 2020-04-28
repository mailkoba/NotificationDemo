import { Component, Inject } from "@angular/core";
import { HttpService } from "../../core/http.service";
import { UrlHelper } from "../../utils/url-helper";
import { ActivatedRoute } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { UserContextDto, LoginInputModel } from "../../models/models";

@Component({
    templateUrl: "./login.component.html"
})
export class LoginComponent {
    message = "";

    readonly model = {
        username: ""
    };

    logins = new Array<string>();

    constructor(private readonly activatedRoute: ActivatedRoute,
        private readonly httpService: HttpService,
        @Inject("BASE_URL") private readonly baseUrl: string) {
        this.httpService.post<UserContextDto[]>(UrlHelper.security.getLoginList)
            .then(logins => {
                this.logins = logins.map(x => x.login);
            });
    }

    async submit() {
        if (this.model.username === "" || this.model.username === null) return;
        this.message = "";

        const loginModel = {
            username: this.model.username
        } as LoginInputModel;

        this.httpService.postDetail(UrlHelper.security.login, loginModel)
            .then(async () => {
                window.location.replace(this.getReturnUrl());
            })
            .catch((error: HttpErrorResponse) => {
                this.message = this.httpService.extractErrorDescription(error);
            });
    }

    selectUser(login: string) {
        this.model.username = login;
    }

    private getReturnUrl(): string {
        const fromQuery = this.activatedRoute.snapshot.queryParams.returnUrl;
        // If the url is comming from the query string, check that is either
        // a relative url or an absolute url
        if (fromQuery &&
            !(fromQuery.startsWith(`${window.location.origin}/`) ||
                /\/[^\/].*/.test(fromQuery) ||
                fromQuery === "/")) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.");
        }
        return fromQuery || this.baseUrl;
    }
}
