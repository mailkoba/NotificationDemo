import { Component } from "@angular/core";
import { PushService } from "../../shared/push/push.service";
import { Observable } from "rxjs";

@Component({
    selector: "send-message-self",
    templateUrl: "./send-message-self.component.html"
})
export class SendMessageSelfComponent {

    readonly subscribed: Observable<boolean>;

    model = {
        title: "",
        body: ""
    };

    constructor(private readonly pushService: PushService) {
        this.subscribed = pushService.hasSubscription;
    }

    async submit() {
        await this.pushService.sendMessageSelf(this.model.title, this.model.body);
    }
}
