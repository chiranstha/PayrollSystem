import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmployeeLevelForViewDto, EmployeeLevelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeLevelModal',
    templateUrl: './view-employeeLevel-modal.component.html',
})
export class ViewEmployeeLevelModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeLevelForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetEmployeeLevelForViewDto();
        this.item.employeeLevel = new EmployeeLevelDto();
    }

    show(item: GetEmployeeLevelForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
