import { Component } from "@angular/core";
import { setTheme } from "ngx-bootstrap/utils";

@Component({
    selector: "body",
    template: `<router-outlet></router-outlet>`
})
export class AppComponent {
    constructor() {
        setTheme("bs4");
    }
}
