import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetGradeUpgradeForViewDto, EmployeeGrade} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewGradeUpgradeModal',
    templateUrl: './view-gradeUpgrade-modal.component.html'
})
export class ViewGradeUpgradeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGradeUpgradeForViewDto;
    employeeGrade = EmployeeGrade;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGradeUpgradeForViewDto();
       
    }

    show(item: GetGradeUpgradeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
