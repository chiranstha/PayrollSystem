import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    PrincipalAllowanceSettingsServiceProxy, PrincipalAllowanceSettingEmployeeLevelLookupTableDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormBuilder, FormGroup } from '@angular/forms';
@Component({
    selector: 'createOrEditPrincipalAllowanceSettingModal',
    templateUrl: './create-or-edit-PrincipalAllowanceSetting-modal.component.html'
})
export class CreateOrEditPrincipalAllowanceSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    form: FormGroup;
    allEmployeeLevels: PrincipalAllowanceSettingEmployeeLevelLookupTableDto[];
    id: string;
    constructor(
        injector: Injector,
        private _PrincipalAllowanceSettingsServiceProxy: PrincipalAllowanceSettingsServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }
    createForm(item: any = {}) {
        this.form = this.fb.group({
            amount: [item.amount || 0],
            employeeLevelId: [item.employeeLevelId || null],
            id: [item.id || null]
        });
    }
    show(PrincipalAllowanceSettingId?: string): void {
        if (PrincipalAllowanceSettingId) {
            this.id = PrincipalAllowanceSettingId;
            this._PrincipalAllowanceSettingsServiceProxy.getPrincipalAllowanceSettingForEdit(PrincipalAllowanceSettingId).subscribe(result => {
                this.createForm(result);
            });
        }
        this.active = true;
        this.modal.show();
    }

    getAllEmployeeLevelForTableDropdown() {
        this._PrincipalAllowanceSettingsServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe(result => {
            this.allEmployeeLevels = result;
        });
    }

    save(): void {
        this.saving = true;
        this._PrincipalAllowanceSettingsServiceProxy.createOrEdit(this.form.getRawValue())
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
        this.getAllEmployeeLevelForTableDropdown();
        this.createForm();
    }
}
