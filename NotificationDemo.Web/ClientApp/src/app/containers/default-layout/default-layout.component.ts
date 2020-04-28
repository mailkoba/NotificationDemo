import { Component } from "@angular/core";
import { UrlHelper } from "../../utils/url-helper";
import { HttpService } from "../../core/http.service";
import { Observable } from "rxjs";
import { ContextService } from "../../core/context.service";
import { UserContextDto } from "../../models/models";

@Component({
    selector: "app-dashboard",
    templateUrl: "./default-layout.component.html"
})
export class DefaultLayoutComponent {
    readonly userContext: Observable<UserContextDto>;
    isCollapsed = true;

    constructor(private readonly httpService: HttpService,
        contextService: ContextService) {
        this.userContext = contextService.context;
    }

    async logout() {
        await this.httpService.post(UrlHelper.security.logout);
        document.location.reload();
    }
}
