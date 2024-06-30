﻿import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSchoolLevelModal',
    templateUrl: './view-schoolLevel-modal.component.html',
})
export class ViewSchoolLevelModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    constructor(injector: Injector) {
        super(injector);
    }

    show(id: string): void {

    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}