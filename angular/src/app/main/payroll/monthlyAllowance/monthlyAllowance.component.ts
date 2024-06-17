import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MonthlyAllowanceServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMonthlyAllowanceModalComponent } from './create-or-edit-monthlyAllowance-modal.component';

import { ViewMonthlyAllowanceModalComponent } from './view-monthlyAllowance-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './monthlyAllowance.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MonthlyAllowancesComponent extends AppComponentBase {
    @ViewChild('createOrEditMonthlyAllowanceModal', { static: true })
    createOrEditMonthlyAllowanceModal: CreateOrEditMonthlyAllowanceModalComponent;
    @ViewChild('viewMonthlyAllowanceModal', { static: true }) viewMonthlyAllowanceModal: ViewMonthlyAllowanceModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';

    constructor(
        injector: Injector,
        private _monthlyAllowancesServiceProxy: MonthlyAllowanceServiceProxy,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMonthlyAllowances(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._monthlyAllowancesServiceProxy
            .getAll()
            .subscribe((result) => {
                this.primengTableHelper.records = result;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createMonthlyAllowance(): void {
        this.createOrEditMonthlyAllowanceModal.show();
    }

    deleteMonthlyAllowance(id: string): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._monthlyAllowancesServiceProxy.delete(id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        // this._monthlyAllowancesServiceProxy.getMonthlyAllowancesToExcel(this.filterText).subscribe((result) => {
        //     this._fileDownloadService.downloadTempFile(result);
        // });
    }

    resetFilters(): void {
        this.filterText = '';

        this.getMonthlyAllowances();
    }
}
