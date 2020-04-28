export interface LoginInputModel {
    username: string;
}

export interface UserContextDto {
    login: string;
    name: string;
}

export interface SubscriptionDto {
    userId: number;
    endpoint: string;
    expirationTime?: number;
    p256Dh: string;
    auth: string;
}

export interface NotificationActionDto {
    action: string;
    title: string;
}

export interface NotificationDto {
    title: string;
    lang: string;
    body: string;
    tag: string;
    image: string;
    icon: string;
    badge: string;
    timestamp: Date;
    requireInteraction: boolean;
    actions: NotificationActionDto[];
}

export interface NotificationGroupDto {
    userIds: number[];
    notification: NotificationDto;
}

export interface UserDto {
    id: number;
    login: string;
    name: string;
}

export interface UserSubscriptionDto {
    user: UserDto;
    subscriptions: SubscriptionDto[];
}
