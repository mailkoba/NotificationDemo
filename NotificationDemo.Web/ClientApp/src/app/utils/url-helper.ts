export class UrlHelper {
    static security = {
        login: "/api/LoginApi/Authenticate",
        logout: "/api/LoginApi/Logout",
        checkAuthorization: "/api/HomeApi/CheckAuthorization",
        getUserContext: "/api/HomeApi/GetUserContext",
        getLoginList: "/api/HomeApi/GetLoginList"
    };
    static push = {
        getVapidPublicKey: "/api/PushApi/GetVapidPublicKey",
        subscribe: "/api/PushApi/Subscribe",
        unsubscribe: "/api/PushApi/Unsubscribe",
        sendMessageSelf: "/api/PushApi/SendMessageSelf",
        sendMessage: "/api/PushApi/SendMessage"
    };
    static users = {
        getUserSubscriptionList: "api/UserApi/GetUserSubscriptionList"
    }
}
