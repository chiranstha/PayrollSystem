﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    GradeUpgradesServiceProxy, CreateOrEditGradeUpgradeDto, GradeUpgradeEmployeeLookupTableDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'createOrEditGradeUpgradeModal',
    templateUrl: './create-or-edit-gradeUpgrade-modal.component.html'
})
export class CreateOrEditGradeUpgradeModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;


    
    allEmployees: GradeUpgradeEmployeeLookupTableDto[];
    id: string;


    constructor(
        injector: Injector,
        private _gradeUpgradesServiceProxy: GradeUpgradesServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({

            dateMiti: [item.dateMiti || '2081/3/20'],
            grade: [item.grade || ''],
            remarks: [item.remarks || ''],
            employeeId: [item.employeeId || null],
            id: [item.id || null]
        });
    }

    show(gradeUpgradeId?: string): void {


        if (gradeUpgradeId) {

            this.id = gradeUpgradeId;

            this._gradeUpgradesServiceProxy.getGradeUpgradeForEdit(gradeUpgradeId).subscribe(result => {

                this.createForm(result)
            });
        }

        this.active = true;
        this.modal.show();

    }

    getAllEmployeeForTableDropdown() {
        this._gradeUpgradesServiceProxy.getAllEmployeeForTableDropdown().subscribe(result => {
            this.allEmployees = result;
        });
    }

    save(): void {
        this.saving = true;



        this._gradeUpgradesServiceProxy.createOrEdit(this.form.getRawValue())
            .pipe(finalize(() => { this.saving = false; }))
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
        this.getAllEmployeeForTableDropdown();
        this.createForm();
    }
}
