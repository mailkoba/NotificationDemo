import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { Observable, Observer } from "rxjs";
import { timeout } from "rxjs/operators";
//import { ModalService } from "../shared/modal/modal.service";
import { ProcessingService } from "../shared/processing/processing.service";

const correctTimezone = (date: Date) => {
    const correctedDate = new Date(date.valueOf());
    correctedDate.setHours(-date.getTimezoneOffset() / 60);
    return correctedDate;
};

Date.prototype.toJSON = function () {
    return correctTimezone(this).toISOString();
};

@Injectable()
export class HttpService {
    private readonly timeout = 120000;   // 120 сек.

    constructor(private readonly httpClient: HttpClient,
  //      private readonly modalService: ModalService,
            private readonly processingService: ProcessingService
  ) {
    }

    /**
     * Выполняет запрос методом POST, возвращает Observable<T>.
     * Результат возвращается только при получении успешного результата (коды 200-299),
     * ошибки обрабатываются и отображаются в модальном окне.
     */
    postObservable<T>(url: string, body?: any, silent: boolean = false): Observable<T> {
        return Observable.create((observer: Observer<T>) => {
            if (!silent) this.processingService.start();
            this.httpClient
                .post<T>(url,
                    JSON.stringify(body),
                    {
                        headers: new HttpHeaders({
                            "Content-Type": "application/json;charset=utf-8"
                        }),
                        responseType: "json"
                    })
                .pipe(timeout(this.timeout))
                .subscribe(
                    response => {
                        observer.next(response);
                        observer.complete();
                        if (!silent) this.processingService.stop();
                    },
                    (error: HttpErrorResponse) => {
                        if (!silent) this.processingService.stop();
                        this.handleError(error);
                    });
        });
    }

    /**
     * Выполняет запрос методом POST, возвращает Promise<T>.
     * Результат возвращается только при получении успешного результата (коды 200-299),
     * ошибки обрабатываются и отображаются в модальном окне.
     */
    post<T>(url: string, body?: any, silent: boolean = false): Promise<T> {
        return new Promise<T>((resolve) => {
            this.postObservable<T>(url, body, silent)
                .subscribe(
                    value => resolve(value),
                    // ReSharper disable once UnusedParameter
                    err => {
                        //empty
                    });
        });
    }

    /**
     * Выполняет запрос скачивания файла методом POST, возвращает Observable<IBlobInfo>.
     * Результат возвращается в любом случае, включая ошибки.
     * Обработка должна производиться в месте вызова метода.
     */
    postBlobObservable(url: string, body?: any): Observable<IBlobInfo> {
        return Observable.create((observer: Observer<any>) => {
            this.processingService.start();
            this.httpClient
                .post(url,
                    JSON.stringify(body),
                    {
                        observe: "response",
                        headers: new HttpHeaders({
                            "Content-Type": "application/json;charset=utf-8"
                        }),
                        responseType: "blob"
                    })
                .pipe(timeout(this.timeout))
                .subscribe((response: any) => {
                    const resp = response as HttpResponse<Blob>;
                    if (resp.ok &&
                        resp.headers.get("Content-Type").indexOf("application/json") === -1) {
                        observer.next(
                            {
                                blob: resp.body,
                                fileName: this.extractFilename(resp.headers.get("Content-Disposition"))
                            } as IBlobInfo);
                        observer.complete();
                    } else {
                        observer.error("Ошибка при получении файла.");
                    }
                    this.processingService.stop();
                },
                    (error: HttpErrorResponse) => {
                        this.processingService.stop();
                        this.handleError(error);
                    });
        });
    }

    /**
     * Выполняет запрос скачивания файла методом POST, возвращает Promise<IBlobInfo>.
     * Результат возвращается только при получении успешного результата (коды 200-299),
     * ошибки обрабатываются и отбражаются в модальном окне.
     */
    postBlob(url: string, body?: any): Promise<IBlobInfo> {
        return new Promise<IBlobInfo>((resolve) => {
            this.postBlobObservable(url, body).subscribe(
                (value: IBlobInfo) => {
                    if (!!value) resolve(value);
                },
                err => {
                    this.handleError(err);
                });
        });
    }

    /**
     * Выполняет запрос методом POST, возвращает Observable<Response>.
     * Результат возвращается в любом случае, включая ошибки.
     * Обработка должна производиться в месте вызова метода.
     */
    postDetailObservable<T>(url: string, body?: any): Observable<HttpResponse<T>> {
        return this.httpClient
            .post<T>(url,
                JSON.stringify(body),
                {
                    observe: "response",
                    headers: new HttpHeaders({
                        "Content-Type": "application/json;charset=utf-8"
                    }),
                    responseType: "json"
                })
            .pipe(timeout(this.timeout));
    }

    /**
     * Выполняет запрос методом POST, возвращает Promise<Response>.
     * Результат возвращается в любом случае, включая ошибки.
     * Обработка должна производиться в месте вызова метода.
     */
    postDetail<T>(url: string, body?: any): Promise<HttpResponse<T>> {
        return this.postDetailObservable<T>(url, body).toPromise();
    }

    /**
     * Возвращает описание ошибки из ответа сервера
     * @param error
     */
    extractErrorDescription(error: HttpErrorResponse): string {
        let msg: string;
        if (error && error.constructor.name === "TimeoutError") {
            msg = "Превышено время ожидания.";
        } else {
            msg = error.error
                ? (error.error instanceof ProgressEvent
                    ? null
                    : (error.error instanceof Object ? error.error : JSON.parse(error.error)).data)
                : null;
        }
        return msg || "Ошибка при обращении к серверу.";
    }

    private handleError(error: HttpErrorResponse) {
        const unauthorized = error.status === HttpCodes.Unauthorized;
        if (unauthorized) {
            window.location.replace("/auth/login");
        } else {
            const errMsg = this.extractErrorDescription(error);
            console.log(errMsg);
         //   this.modalService.showError("Ошибка", errMsg);
        }
    }

    //todo: при возможности переделать на regex
    private extractFilename(disposition: string): string {
        const filenameString = "filename*=utf-8''";
        const res = disposition.toLowerCase().indexOf(filenameString);
        if (res === -1) {
            const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
            const matches = filenameRegex.exec(disposition);
            if (matches != null && matches[1]) {
                return matches[1].replace(/['"]/g, "");
            }
        } else {
            const end = disposition.indexOf(";", res);
            return decodeURI(
                disposition.substring(
                    res + filenameString.length,
                    end === -1 ? disposition.length : end));
        }
        return "";
    }
}

export enum HttpCodes {
    Ok = 200,
    Unauthorized = 401,
    NeedPasswordChange = 402,
    Forbidden = 403,
    InternalServerError = 500
}

export class ContainerDto<TData> {
    data: TData;
}

export interface IBlobInfo {
    blob: Blob;
    fileName: string;
}
