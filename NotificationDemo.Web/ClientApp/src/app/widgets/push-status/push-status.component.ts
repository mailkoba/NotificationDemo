import { Component } from "@angular/core";
import { Observable } from "rxjs";
import { PushService } from "../../shared/push/push.service";

@Component({
    selector: "push-status",
    templateUrl: "./push-status.component.html"
})
export class PushStatusComponent {

    readonly subscribed: Observable<boolean>;

    constructor(private readonly pushService: PushService) {
        this.subscribed = pushService.hasSubscription;
    }

    async subscribe() {
        await this.pushService.subscribe();
    }

    async unsubscribe() {
        await this.pushService.unsubscribe();
    }
}
