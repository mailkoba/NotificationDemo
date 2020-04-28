import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ProcessingComponent } from "./processing.component";
import { ProcessingService } from "./processing.service";

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        ProcessingComponent
    ],
    exports: [
        ProcessingComponent
    ],
    providers: [ProcessingService]
})
export class ProcessingModule {
}
