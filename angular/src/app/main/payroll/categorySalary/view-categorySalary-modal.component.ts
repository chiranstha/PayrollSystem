import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetCategorySalaryForViewDto,
    CategorySalaryDto,
    EmployeeCategory,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCategorySalaryModal',
    templateUrl: './view-categorySalary-modal.component.html',
})
export class ViewCategorySalaryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCategorySalaryForViewDto;
    employeeCategory = EmployeeCategory;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetCategorySalaryForViewDto();
        this.item.categorySalary = new CategorySalaryDto();
    }

    show(item: GetCategorySalaryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
