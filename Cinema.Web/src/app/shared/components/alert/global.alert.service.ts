import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface GlobalAlert {
    type: 'success' | 'info' | 'warning' | 'error';
    message: string;
}

@Injectable({ providedIn: 'root' })
export class GlobalAlertService {
    private alertSubject = new Subject<GlobalAlert>();

    get alert$(): Observable<GlobalAlert> {
        return this.alertSubject.asObservable();
    }

    show(type: GlobalAlert['type'], message: string) {
        this.alertSubject.next({ type, message });
    }
}