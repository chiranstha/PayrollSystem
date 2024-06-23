import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { FestivalBonusSettingsServiceProxy, CreateOrEditFestivalBonusSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormGroup, FormBuilder } from '@angular/forms';



@Component({
    selector: 'createOrEditFestivalBonusSettingModal',
    templateUrl: './create-or-edit-festivalBonusSetting-modal.component.html'
})
export class CreateOrEditFestivalBonusSettingModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;
    id: string;





    constructor(
        injector: Injector,
        private _festivalBonusSettingsServiceProxy: FestivalBonusSettingsServiceProxy,
        private _dateTimeService: DateTimeService, private fb: FormBuilder,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.createForm();
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({
            monthId: [item.monthId || ''],
            percentOrAmount: [item.percentOrAmount || ''],
            value: [item.value || 0],
            remarks: [item.remarks || ''],
            id: [item.id || null]
        });
    }

    show(festivalBonusSettingId?: string): void {


        if (festivalBonusSettingId) {

            this.id = festivalBonusSettingId;
            this._festivalBonusSettingsServiceProxy.getFestivalBonusSettingForEdit(festivalBonusSettingId).subscribe(result => {
                this.createForm(result);
            });
        }

        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;



        this._festivalBonusSettingsServiceProxy.createOrEdit(this.form.getRawValue())
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


}
