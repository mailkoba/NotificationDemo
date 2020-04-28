import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class ProcessingService {
    private numOfProcesses = 0;
    showProcessing = new BehaviorSubject<boolean>(false);

    start() {
        this.numOfProcesses++;
        this.update();
    }

    stop() {
        this.numOfProcesses--;
        if (this.numOfProcesses < 0) {
            this.numOfProcesses = 0;
        }
        this.update();
    }

    reset() {
        this.numOfProcesses = 0;
        this.update();
    }

    private update() {
        const show = this.numOfProcesses > 0;
        this.showProcessing.next(show);
    }
}
