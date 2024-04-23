import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    InternalGradeSetupServiceProxy,
    EmployeeCategory,
    EmployeeGrade,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditInternalGradeSetupModalComponent } from './create-or-edit-internalGradeSetup-modal.component';

import { ViewInternalGradeSetupModalComponent } from './view-internalGradeSetup-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './internalGradeSetup.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class InternalGradeSetupComponent extends AppComponentBase {
    @ViewChild('createOrEditInternalGradeSetupModal', { static: true })
    createOrEditInternalGradeSetupModal: CreateOrEditInternalGradeSetupModalComponent;
    @ViewChild('viewInternalGradeSetupModal', { static: true })
    viewInternalGradeSetupModal: ViewInternalGradeSetupModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';

    employeeCategory = EmployeeCategory;
    employeeGrade = EmployeeGrade;

    constructor(
        injector: Injector,
        private _internalGradeSetupServiceProxy: InternalGradeSetupServiceProxy,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getInternalGradeSetup(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._internalGradeSetupServiceProxy
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

    createInternalGradeSetup(): void {
        this.createOrEditInternalGradeSetupModal.show();
    }

    deleteInternalGradeSetup(id): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._internalGradeSetupServiceProxy.delete(id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._internalGradeSetupServiceProxy.getInternalGradeSetupToExcel(this.filterText).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
        });
    }

    resetFilters(): void {
        this.filterText = '';

        this.getInternalGradeSetup();
    }
}
