import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SchoolInfosServiceProxy, CreateOrEditSchoolInfoDto, UniversalDropdownDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileUploader, FileUploaderOptions } from '@node_modules/ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';

import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'createOrEditSchoolInfoModal',
    templateUrl: './create-or-edit-schoolInfo-modal.component.html',
})
export class CreateOrEditSchoolInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    form: FormGroup;


    schoolLevels: UniversalDropdownDto[];
    imageFileUploader: FileUploader;
    imageFileToken: string;
    imageFileName: string;
    imageFileAcceptedTypes = '';
    id: string;

    constructor(
        injector: Injector,
        private _schoolInfosServiceProxy: SchoolInfosServiceProxy,
        private _dateTimeService: DateTimeService,
        private _http: HttpClient,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({
            name: [item.name || ''],
            address: [item.address || ''],
            phoneNo: [item.phoneNo || ''],
            email: [item.email || ''],
            wardNo: [item.wardNo || 0],
            description: [item.description || ''],
            schooolLevelId: [item.schooolLevelId || ''],
            image: [item.image || ''],
            imageToken: [item.imageToken || ''],
            id: [item.id || null]
        });
    }

    show(schoolInfoId?: string): void {
        if (schoolInfoId) {

            this.id = schoolInfoId;
            this._schoolInfosServiceProxy.getSchoolInfoForEdit(schoolInfoId).subscribe((result) => {
                this.createForm(result);
                this.active = true;
                this.modal.show();
            });
        }


        this.imageFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + '/SchoolInfos/UploadimageFile',
            (fileToken) => (this.imageFileToken = fileToken)
        );

        this.active = true;
        this.modal.show();
    }

    getAllSchoolLevels() {
        this._schoolInfosServiceProxy.getAllSchoolLevels().subscribe((res) => {
            this.schoolLevels = res;
        })
    }

    save(): void {
        this.saving = true;

        this.form.get('imageToken').setValue(this.imageFileToken);

        this._schoolInfosServiceProxy
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

    onSelectImageFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.imageFileUploader.clearQueue();
        this.imageFileUploader.addToQueue([selectedFile]);
        this.imageFileUploader.uploadAll();
    }

    removeImageFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._schoolInfosServiceProxy.removeImageFile(this.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.imageFileName = null;
                });
            }
        });
    }

    initializeUploader(url: string, onSuccess: (fileToken: string) => void): FileUploader {
        let uploader = new FileUploader({ url: url });

        let _uploaderOptions: FileUploaderOptions = {};
        _uploaderOptions.autoUpload = false;
        //_uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        _uploaderOptions.removeAfterUpload = true;

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            // const resp = <IAjaxResponse>JSON.parse(response);
            // if (resp.success && resp.result.fileToken) {
            //     onSuccess(resp.result.fileToken);
            // } else {
            //     this.message.error(resp.result.message);
            // }
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this.getAllSchoolLevels();
        this.createForm();
        this._http
            .get(AppConsts.remoteServiceBaseUrl + '/schoolInfos/GetImageFileAllowedTypes')
            .subscribe((data: any) => {
                if (!data || !data.result) {
                    return;
                }

                let list = data.result as string[];
                if (list.length === 0) {
                    return;
                }

                for (let i = 0; i < list.length; i++) {
                    this.imageFileAcceptedTypes += '.' + list[i] + ',';
                }
            });
    }
}
