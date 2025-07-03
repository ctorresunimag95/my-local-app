import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { Subscription } from 'rxjs';
import { GlobalAlert, GlobalAlertService } from './global.alert.service';

@Component({
  selector: 'app-global-alert',
  standalone: true,
  imports: [CommonModule, NzAlertModule],
  template: `
    <nz-alert
      *ngIf="visible"
      [nzType]="type"
      [nzMessage]="message"
      nzShowIcon
      nzCloseable
      (nzOnClose)="visible = false"
      style="position: fixed; top: 24px; right: 24px; z-index: 1000; width: 320px;">
    </nz-alert>
  `
})
export class GlobalAlertComponent implements OnInit, OnDestroy {
  type: GlobalAlert['type'] = 'info';
  message = '';
  visible = false;
  private sub?: Subscription;

  constructor(private alertService: GlobalAlertService) { }

  ngOnInit() {
    this.sub = this.alertService.alert$.subscribe(alert => {
      this.type = alert.type;
      this.message = alert.message;
      this.visible = true;
    });
  }

  ngOnDestroy() {
    this.sub?.unsubscribe();
  }
}