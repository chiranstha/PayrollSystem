import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SchoolLevelServiceProxy, CreateOrEditSchoolLevelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditSchoolLevelModal',
    templateUrl: './create-or-edit-schoolLevel-modal.component.html',
})
export class CreateOrEditSchoolLevelModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    schoolLevel: CreateOrEditSchoolLevelDto = new CreateOrEditSchoolLevelDto();

    constructor(
        injector: Injector,
        private _schoolLevelsServiceProxy: SchoolLevelServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(schoolLevelId?: string): void {
        if (!schoolLevelId) {
            this.schoolLevel = new CreateOrEditSchoolLevelDto();
            this.schoolLevel.id = schoolLevelId;

            this.active = true;
            this.modal.show();
        } else {
            this._schoolLevelsServiceProxy.getSchoolLevelForEdit(schoolLevelId).subscribe((result) => {

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._schoolLevelsServiceProxy
            .createOrEdit(this.schoolLevel)
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

    ngOnInit(): void { }
}
