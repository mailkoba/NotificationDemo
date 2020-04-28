import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HomeComponent } from "../home/home.component";
import { HomeRoutingModule } from "./home-routing.module";
import { WidgetsModule } from "../../widgets/widgets.module";

@NgModule({
    declarations: [HomeComponent],
    imports: [
        CommonModule,
        HomeRoutingModule,
        WidgetsModule
    ]
})
export class HomeModule {
}
