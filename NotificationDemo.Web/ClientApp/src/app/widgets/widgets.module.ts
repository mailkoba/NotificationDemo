import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PushStatusComponent } from "./push-status/push-status.component";
import { AlertModule } from "ngx-bootstrap/alert";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { SendMessageSelfComponent } from "./send-message-self/send-message-self.component";
import { FormsModule } from "@angular/forms";
import { UsersComponent } from "./users/users.component";

const components = [
    PushStatusComponent,
    SendMessageSelfComponent,
    UsersComponent
];

@NgModule({
    declarations: components,
    exports: components,
    imports: [
        CommonModule,
        FormsModule,
        AlertModule,
        ButtonsModule
    ]
})
export class WidgetsModule {
}
