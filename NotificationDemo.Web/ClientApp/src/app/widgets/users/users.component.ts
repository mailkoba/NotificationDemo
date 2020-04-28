import { Component, } from "@angular/core";
import { UserSubscriptionDto } from "../../models/models";
import { HttpService } from "../../core/http.service";
import { UrlHelper } from "../../utils/url-helper";
import { PushService } from "../../shared/push/push.service";

@Component({
    selector: "users",
    templateUrl: "./users.component.html"
})
export class UsersComponent {
    users = new Array<UserSubscriptionDto>();

    model = {
        title: "",
        body: ""
    };

    constructor(readonly httpService: HttpService,
        private readonly pushService: PushService) {
        httpService.post<UserSubscriptionDto[]>(UrlHelper.users.getUserSubscriptionList)
            .then(x => this.users = x);
    }

    async submit() {
        const userIds = this.users.filter(x => (x as any).selected).map(x => x.user.id);
        if (userIds.length === 0) {
            return;
        }
        await this.pushService.sendMessage(userIds, this.model.title, this.model.body);
    }
}
