import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CreateOrEditMontlyAllowanceDto, MonthlyAllowanceServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'createOrEditMonthlyAllowanceModal',
    templateUrl: './create-or-edit-monthlyAllowance-modal.component.html',
})
export class CreateOrEditMonthlyAllowanceModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;
    id: string;

    constructor(
        injector: Injector,
        private _monthlyAllowancesServiceProxy: MonthlyAllowanceServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({
          
            name: [item.name || ''],
            amount: [item.amount || 0],
            employeeCategory: [item.employeeCategory || null],
            id: [item.id || null]

            
        });
    }

    show(monthlyAllowanceId?: string): void {
        if (monthlyAllowanceId) {
           
            this.id = monthlyAllowanceId;           
            this._monthlyAllowancesServiceProxy.getMonthlyAllowanceForEdit(monthlyAllowanceId).subscribe((result) => {
                this.createForm(result);
                this.active = true;
                this.modal.show();
            });
        }
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;

        this._monthlyAllowancesServiceProxy
            .createOrEdit(this.form.getRawValue())
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this.createForm();
     }
}
