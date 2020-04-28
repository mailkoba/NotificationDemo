import { NgModule, ModuleWithProviders, Optional, SkipSelf } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HttpService } from "./http.service";
import { ContextService } from "./context.service";

@NgModule({
    imports: [
        CommonModule
    ],
    providers: [
        HttpService,
        ContextService
    ]
})
export class CoreModule {
    static forRoot(): ModuleWithProviders<CoreModule> {
        return {
            ngModule: CoreModule,
            providers: [
                HttpService,
                ContextService
            ]
        };
    }

    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        if (parentModule) {
            throw new Error("CoreModule is already loaded. Import it in the AppModule only");
        }
    }
}
