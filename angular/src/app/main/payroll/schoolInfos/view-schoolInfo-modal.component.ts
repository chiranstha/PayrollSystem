import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetSchoolInfoForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSchoolInfoModal',
    templateUrl: './view-schoolInfo-modal.component.html',
})
export class ViewSchoolInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSchoolInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetSchoolInfoForViewDto();

    }

    show(item: GetSchoolInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
