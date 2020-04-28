import { Injectable } from "@angular/core";
import { HttpService, ContainerDto } from "../../core/http.service";
import { UrlHelper } from "../../utils/url-helper";
import { SubscriptionDto, NotificationDto, NotificationGroupDto, NotificationDto as INotificationDto } from "../../models/models";
import { BehaviorSubject, Observable, concat, Observer } from "rxjs";
import { take, filter, tap, share } from "rxjs/operators";

@Injectable({
    providedIn: "root"
})
export class PushService {

    private subscribeSubject = new BehaviorSubject<boolean | null>(null);

    get hasSubscription(): Observable<boolean> {
        return concat(
            this.subscribeSubject.pipe(take(1), filter(x => x !== null)),
            this.isSubscribed().pipe(tap(x => this.subscribeSubject.next(x))),
            this.subscribeSubject.asObservable()
        ).pipe(share());
    }

    constructor(private readonly httpService: HttpService) {
        this.init();
    }

    private notificationEnabled = false;

    async subscribe(): Promise<SubscriptionDto> {
        if (!this.notificationEnabled) return null;

        const registration = await navigator.serviceWorker.ready;

        let subscription = await registration.pushManager.getSubscription();

        // renew subscription if we're within 5 days of expiration
        if (subscription &&
            subscription.expirationTime &&
            Date.now() > subscription.expirationTime - 432000000) {
            await subscription.unsubscribe();
            await this.httpService.post<SubscriptionDto>(UrlHelper.push.unsubscribe,
                {
                    subscription: subscription
                });
            subscription = null;
            this.subscribeSubject.next(false);
        }

        if (subscription) {
            this.subscribeSubject.next(true);
            return {
                userId: 0,
                endpoint: subscription.endpoint,
                auth: subscription.getKey("auth").toString(),
                p256Dh: subscription.getKey("p256dh").toString(),
                expirationTime: subscription.expirationTime
            } as SubscriptionDto;
        }

        const vapidPublicKey = await this.httpService.post<ContainerDto<string>>(UrlHelper.push.getVapidPublicKey);

        subscription = await registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: this.urlBase64ToUint8Array(vapidPublicKey.data)
        });

        const result = await this.httpService.post<SubscriptionDto>(UrlHelper.push.subscribe,
            {
                subscription: subscription
            });

        this.subscribeSubject.next(true);

        return result;
    }

    async unsubscribe() {
        if (!this.notificationEnabled) return;

        const registration = await navigator.serviceWorker.ready;
        const subscription = await registration.pushManager.getSubscription();

        if (subscription) {
            await subscription.unsubscribe();
            await this.httpService.post<SubscriptionDto>(UrlHelper.push.unsubscribe,
                {
                    subscription: subscription
                });
            this.subscribeSubject.next(false);
        }
    }

    async sendMessageSelf(title: string, body: string) {
        if (!this.notificationEnabled) return;

        await this.httpService.post<SubscriptionDto>(UrlHelper.push.sendMessageSelf,
            {
                title: title,
                body: body
            } as NotificationDto);
    }

    async sendMessage(userIds: number[], title: string, body: string) {
        if (!this.notificationEnabled) return;

        await this.httpService.post<SubscriptionDto>(UrlHelper.push.sendMessage,
            {
                notification: {
                    title: title,
                    body: body
                } as INotificationDto,
                userIds: userIds
            } as NotificationGroupDto
        );
    }

    private isSubscribed(): Observable<boolean> {
        return Observable.create((observer: Observer<boolean>) => {
            if (!this.notificationEnabled) {
                observer.next(false);
                observer.complete();
            } else {
                navigator.serviceWorker.ready
                    .then(registration => registration.pushManager.getSubscription())
                    .then(subscription => {
                        observer.next(subscription !== null);
                        observer.complete();
                    });
            }
        });
    }

    private init() {
        if (navigator.serviceWorker && "Notification" in window) {
            this.notificationEnabled = window.Notification.permission === "granted";
            if (!this.notificationEnabled) {
                window.Notification.requestPermission().then(permission => {
                    this.notificationEnabled = permission === "granted";
                    if (!this.notificationEnabled) {
                        alert("Notification not enabled!");
                    } else {
                        navigator.serviceWorker.register("/service-worker.js");
                    }
                });
            }
        }
    }

    private urlBase64ToUint8Array(base64String: string): Uint8Array {
        const padding = "=".repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding)
            .replace(/\-/g, "+")
            .replace(/_/g, "/");

        const rawData = window.atob(base64);
        const outputArray = new Uint8Array(rawData.length);

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }

        return outputArray;
    }
}
