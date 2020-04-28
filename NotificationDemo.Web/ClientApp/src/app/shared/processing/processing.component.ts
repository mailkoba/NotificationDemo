import { Component } from "@angular/core";
import { ProcessingService } from "./processing.service";

@Component({
    selector: "processing",
    template: `
        <div class="child-window" [class.visible]="processingService.showProcessing | async">
            <div class="placeholder">
                <span class="spinner">Загрузка...</span>
            </div>
        </div>
`
})
export class ProcessingComponent {
    constructor(readonly processingService: ProcessingService) {
    }
}
