import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CategorySalaryServiceProxy,
    CreateOrEditCategorySalaryDto,
    CategorySalaryEmployeeLevelLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCategorySalaryModal',
    templateUrl: './create-or-edit-categorySalary-modal.component.html',
})
export class CreateOrEditCategorySalaryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    categorySalary: CreateOrEditCategorySalaryDto = new CreateOrEditCategorySalaryDto();

    employeeLevelName = '';

    allEmployeeLevels: CategorySalaryEmployeeLevelLookupTableDto[];

    constructor(
        injector: Injector,
        private _categorySalaryServiceProxy: CategorySalaryServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(categorySalaryId?: string): void {
        if (!categorySalaryId) {
            this.categorySalary = new CreateOrEditCategorySalaryDto();
            this.categorySalary.id = categorySalaryId;
            this.employeeLevelName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._categorySalaryServiceProxy.getCategorySalaryForEdit(categorySalaryId).subscribe((result) => {

                this.active = true;
                this.modal.show();
            });
        }
        this._categorySalaryServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
    }

    save(): void {
        this.saving = true;

        this._categorySalaryServiceProxy
            .createOrEdit(this.categorySalary)
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
