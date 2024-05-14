import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetPrincipalAllowanceSettingForViewDto } from "@shared/service-proxies/service-proxies";

@Component({
    selector: 'viewPrincipalAllowanceSettingModal',
    templateUrl: './view-PrincipalAllowanceSetting-modal.component.html'
})
export class ViewPrincipalAllowanceSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPrincipalAllowanceSettingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPrincipalAllowanceSettingForViewDto();
    }

    show(item: GetPrincipalAllowanceSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
