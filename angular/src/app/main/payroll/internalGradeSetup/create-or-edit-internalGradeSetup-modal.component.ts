﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    InternalGradeSetupServiceProxy,
    CreateOrEditInternalGradeSetupDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'createOrEditInternalGradeSetupModal',
    templateUrl: './create-or-edit-internalGradeSetup-modal.component.html',
})
export class CreateOrEditInternalGradeSetupModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;
    id: string;
    

    constructor(
        injector: Injector,
        private _internalGradeSetupServiceProxy: InternalGradeSetupServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }
    createForm(item: any = {}) {
        this.form = this.fb.group({
          
            category: [item.category || ''],
            grade: [item.grade || ''],
            isPercent: [item.isPercent || false],
            value: [item.value || 0],
            id: [item.id || null]

            
        });
    }
    show(internalGradeSetupId?: string): void {
        if (internalGradeSetupId) {
           
            this.id = internalGradeSetupId;

          
            this._internalGradeSetupServiceProxy
                .getInternalGradeSetupForEdit(internalGradeSetupId)
                .subscribe((result) => {
                    this.active = true;
                    this.modal.show();
                });
        }
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;

        this._internalGradeSetupServiceProxy
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
