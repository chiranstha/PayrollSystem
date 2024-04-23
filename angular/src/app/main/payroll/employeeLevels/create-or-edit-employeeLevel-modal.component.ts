import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeeLevelsServiceProxy, CreateOrEditEmployeeLevelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditEmployeeLevelModal',
    templateUrl: './create-or-edit-employeeLevel-modal.component.html',
})
export class CreateOrEditEmployeeLevelModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeLevel: CreateOrEditEmployeeLevelDto = new CreateOrEditEmployeeLevelDto();

    constructor(
        injector: Injector,
        private _employeeLevelsServiceProxy: EmployeeLevelsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(employeeLevelId?: string): void {
        if (!employeeLevelId) {
            this.employeeLevel = new CreateOrEditEmployeeLevelDto();
            this.employeeLevel.id = employeeLevelId;

            this.active = true;
            this.modal.show();
        } else {
            this._employeeLevelsServiceProxy.getEmployeeLevelForEdit(employeeLevelId).subscribe((result) => {
                this.employeeLevel = result.employeeLevel;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._employeeLevelsServiceProxy
            .createOrEdit(this.employeeLevel)
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

    ngOnInit(): void {}
}
