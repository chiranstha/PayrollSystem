﻿import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SchoolLevelServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditSchoolLevelModalComponent } from './create-or-edit-schoolLevel-modal.component';

import { ViewSchoolLevelModalComponent } from './view-schoolLevel-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './schoolLevels.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SchoolLevelsComponent extends AppComponentBase {
    @ViewChild('createOrEditSchoolLevelModal', { static: true })
    createOrEditSchoolLevelModal: CreateOrEditSchoolLevelModalComponent;
    @ViewChild('viewSchoolLevelModal', { static: true }) viewSchoolLevelModal: ViewSchoolLevelModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';

    constructor(
        injector: Injector,
        private _schoolLevelsServiceProxy: SchoolLevelServiceProxy,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getSchoolLevels(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._schoolLevelsServiceProxy
            .getAll(
                this.filterText,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createSchoolLevel(): void {
        this.createOrEditSchoolLevelModal.show();
    }

    deleteSchoolLevel(id: string): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._schoolLevelsServiceProxy.delete(id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        // this._schoolLevelsServiceProxy.getSchoolLevelsToExcel(this.filterText).subscribe((result) => {
        //     this._fileDownloadService.downloadTempFile(result);
        // });
    }

    resetFilters(): void {
        this.filterText = '';

        this.getSchoolLevels();
    }
}
