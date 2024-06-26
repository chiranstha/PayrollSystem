﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SchoolLevelServiceProxy, CreateOrEditSchoolLevelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'createOrEditSchoolLevelModal',
    templateUrl: './create-or-edit-schoolLevel-modal.component.html',
})
export class CreateOrEditSchoolLevelModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;
    id: string;
    

    constructor(
        injector: Injector,
        private _schoolLevelsServiceProxy: SchoolLevelServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({
            aliasName: [item.aliasName || ''],
            name: [item.name || ''],
            id: [item.id || null]            
        });
    }

    show(schoolLevelId?: string): void {
        if (schoolLevelId) {           
            this.id = schoolLevelId;
            this._schoolLevelsServiceProxy.getSchoolLevelForEdit(schoolLevelId).subscribe((result) => {
             this.createForm(result);
               
            });
        }
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;

        this._schoolLevelsServiceProxy
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
