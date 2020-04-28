import { Injectable } from "@angular/core";
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from "@angular/router";
import { Observable } from "rxjs";
import { AuthorizeService } from "./authorize.service";

@Injectable({
    providedIn: "root"
})
export class AuthorizeGuard implements CanActivate, CanActivateChild {
    constructor(private readonly router: Router,
        private readonly authorizeService: AuthorizeService) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<boolean> | Promise<boolean> | boolean {
        if (route.url.length > 0 && route.url[0].path.toLowerCase() === "login") {
            return true;
        }

        return this.getAuthPromise(state);
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<boolean> | Promise<boolean> | boolean {
        return this.getAuthPromise(state);
    }

    private getAuthPromise(state: RouterStateSnapshot): Promise<boolean> {
        return new Promise(resolve => {
            this.authorizeService.isAuthenticated()
                .then(result => {
                    if (!result) {
                        this.router.navigate(["/login"],
                            {
                                queryParams: {
                                    ["returnUrl"]: state.url
                                }
                            });
                    }
                    resolve(result);
                });
        });
    }
}
