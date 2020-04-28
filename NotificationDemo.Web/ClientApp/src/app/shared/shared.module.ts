import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { CollapseModule } from "ngx-bootstrap/collapse";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";

import { ProcessingModule } from "./processing/processing.module";
import { PushService } from "./push/push.service";

@NgModule({
    declarations: [],
    exports: [
        ProcessingModule
    ],
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        TooltipModule,
        CollapseModule,
        BsDatepickerModule,
        ProcessingModule
    ],
    providers: [PushService]
})
export class SharedModule {
}
