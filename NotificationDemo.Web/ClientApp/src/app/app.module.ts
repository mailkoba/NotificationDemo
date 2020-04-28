import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppComponent } from "./app.component";

import { defineLocale } from "ngx-bootstrap/chronos";
import { ruLocale } from "ngx-bootstrap/locale";
import { TabsModule } from "ngx-bootstrap/tabs";
import { AlertModule } from "ngx-bootstrap/alert";
import { CarouselModule } from "ngx-bootstrap/carousel";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CollapseModule } from "ngx-bootstrap/collapse";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { PopoverModule } from "ngx-bootstrap/popover";
import { ProgressbarModule } from "ngx-bootstrap/progressbar";
import { ModalModule } from "ngx-bootstrap/modal";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";

import { AppRoutingModule } from "./app-routing.module";
import { DefaultLayoutComponent } from "./containers/default-layout/default-layout.component";
import { CoreModule } from "./core/core.module";
import { AuthorizeService } from "./auth/authorize.service";
import { LoginComponent } from "./auth/login/login.component";
import { SharedModule } from "./shared/shared.module";
import { WidgetsModule } from "./widgets/widgets.module";

defineLocale("ru", ruLocale);

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        DefaultLayoutComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
        HttpClientModule,
        FormsModule,
        AppRoutingModule,
        CoreModule.forRoot(),
        SharedModule,
        BrowserAnimationsModule,
        BsDropdownModule.forRoot(),
        TabsModule.forRoot(),
        BsDropdownModule.forRoot(),
        CarouselModule.forRoot(),
        CollapseModule.forRoot(),
        PaginationModule.forRoot(),
        PopoverModule.forRoot(),
        ProgressbarModule.forRoot(),
        TooltipModule.forRoot(),
        AlertModule.forRoot(),
        ButtonsModule.forRoot(),
        BsDatepickerModule.forRoot(),
        WidgetsModule
    ],
    providers: [AuthorizeService],
    bootstrap: [AppComponent]
})
export class AppModule {
}
