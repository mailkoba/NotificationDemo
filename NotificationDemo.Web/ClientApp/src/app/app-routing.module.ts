import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DefaultLayoutComponent } from "./containers/default-layout/default-layout.component";
import { LoginComponent } from "./auth/login/login.component";
import { AuthorizeGuard } from "./auth/authorize.guard";

export const routes: Routes = [
    {
        path: "",
        redirectTo: "home",
        pathMatch: "full",
    },
    {
        path: "login",
        component: LoginComponent
    },
    {
        path: "",
        component: DefaultLayoutComponent,
        canActivate: [AuthorizeGuard],
        canActivateChild: [AuthorizeGuard],
        children: [
            {
                path: "home",
                loadChildren: () => import("./pages/home/home.module").then(m => m.HomeModule)
            },
            {
                path: "admin",
                loadChildren: () => import("./pages/admin/admin.module").then(m => m.AdminModule)
            }
        ]
    },
    {
        path: "**",
        redirectTo: "home"
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
