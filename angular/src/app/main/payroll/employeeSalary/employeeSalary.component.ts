import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeSalaryServiceProxy,  Months } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './employeeSalary.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class EmployeeSalaryComponent extends AppComponentBase {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    monthFilter = -1;
    maxTechnicalAmountFilter: number;
    maxTechnicalAmountFilterEmpty: number;
    minTechnicalAmountFilter: number;
    minTechnicalAmountFilterEmpty: number;
    maxTotalGradeAmountFilter: number;
    maxTotalGradeAmountFilterEmpty: number;
    minTotalGradeAmountFilter: number;
    minTotalGradeAmountFilterEmpty: number;
    maxInsuranceAmountFilter: number;
    maxInsuranceAmountFilterEmpty: number;
    minInsuranceAmountFilter: number;
    minInsuranceAmountFilterEmpty: number;
    maxTotalSalaryFilter: number;
    maxTotalSalaryFilterEmpty: number;
    minTotalSalaryFilter: number;
    minTotalSalaryFilterEmpty: number;
    maxDearnessAllowanceFilter: number;
    maxDearnessAllowanceFilterEmpty: number;
    minDearnessAllowanceFilter: number;
    minDearnessAllowanceFilterEmpty: number;
    maxTotalWithAllowanceFilter: number;
    maxTotalWithAllowanceFilterEmpty: number;
    minTotalWithAllowanceFilter: number;
    minTotalWithAllowanceFilterEmpty: number;
    maxGovernmentAmountFilter: number;
    maxGovernmentAmountFilterEmpty: number;
    minGovernmentAmountFilter: number;
    minGovernmentAmountFilterEmpty: number;
    maxInternalAmountFilter: number;
    maxInternalAmountFilterEmpty: number;
    minInternalAmountFilter: number;
    minInternalAmountFilterEmpty: number;
    maxPaidSalaryAmountFilter: number;
    maxPaidSalaryAmountFilterEmpty: number;
    minPaidSalaryAmountFilter: number;
    minPaidSalaryAmountFilterEmpty: number;
    schoolInfoNameFilter = '';
    employeeNameFilter = '';
    employeeLevelNameFilter = '';

    months = Months;

    constructor(
        injector: Injector,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getEmployeeSalary(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeSalaryServiceProxy
            .getAll(
                this.filterText,
                this.monthFilter,
                this.maxTechnicalAmountFilter == null
                    ? this.maxTechnicalAmountFilterEmpty
                    : this.maxTechnicalAmountFilter,
                this.minTechnicalAmountFilter == null
                    ? this.minTechnicalAmountFilterEmpty
                    : this.minTechnicalAmountFilter,
                this.maxTotalGradeAmountFilter == null
                    ? this.maxTotalGradeAmountFilterEmpty
                    : this.maxTotalGradeAmountFilter,
                this.minTotalGradeAmountFilter == null
                    ? this.minTotalGradeAmountFilterEmpty
                    : this.minTotalGradeAmountFilter,
                this.maxInsuranceAmountFilter == null
                    ? this.maxInsuranceAmountFilterEmpty
                    : this.maxInsuranceAmountFilter,
                this.minInsuranceAmountFilter == null
                    ? this.minInsuranceAmountFilterEmpty
                    : this.minInsuranceAmountFilter,
                this.maxTotalSalaryFilter == null ? this.maxTotalSalaryFilterEmpty : this.maxTotalSalaryFilter,
                this.minTotalSalaryFilter == null ? this.minTotalSalaryFilterEmpty : this.minTotalSalaryFilter,
                this.maxDearnessAllowanceFilter == null
                    ? this.maxDearnessAllowanceFilterEmpty
                    : this.maxDearnessAllowanceFilter,
                this.minDearnessAllowanceFilter == null
                    ? this.minDearnessAllowanceFilterEmpty
                    : this.minDearnessAllowanceFilter,
                this.maxTotalWithAllowanceFilter == null
                    ? this.maxTotalWithAllowanceFilterEmpty
                    : this.maxTotalWithAllowanceFilter,
                this.minTotalWithAllowanceFilter == null
                    ? this.minTotalWithAllowanceFilterEmpty
                    : this.minTotalWithAllowanceFilter,
                this.maxGovernmentAmountFilter == null
                    ? this.maxGovernmentAmountFilterEmpty
                    : this.maxGovernmentAmountFilter,
                this.minGovernmentAmountFilter == null
                    ? this.minGovernmentAmountFilterEmpty
                    : this.minGovernmentAmountFilter,
                this.maxInternalAmountFilter == null ? this.maxInternalAmountFilterEmpty : this.maxInternalAmountFilter,
                this.minInternalAmountFilter == null ? this.minInternalAmountFilterEmpty : this.minInternalAmountFilter,
                this.maxPaidSalaryAmountFilter == null
                    ? this.maxPaidSalaryAmountFilterEmpty
                    : this.maxPaidSalaryAmountFilter,
                this.minPaidSalaryAmountFilter == null
                    ? this.minPaidSalaryAmountFilterEmpty
                    : this.minPaidSalaryAmountFilter,
                this.schoolInfoNameFilter,
                this.employeeNameFilter,
                this.employeeLevelNameFilter,
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

    createEmployeeSalary(): void {
        this._router.navigate(['/app/main/payroll/employeeSalary/createOrEdit']);
    }

    deleteEmployeeSalary(id: string): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._employeeSalaryServiceProxy.delete(id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._employeeSalaryServiceProxy
            .getEmployeeSalaryToExcel(
                this.filterText,
                this.monthFilter,
                this.maxTechnicalAmountFilter == null
                    ? this.maxTechnicalAmountFilterEmpty
                    : this.maxTechnicalAmountFilter,
                this.minTechnicalAmountFilter == null
                    ? this.minTechnicalAmountFilterEmpty
                    : this.minTechnicalAmountFilter,
                this.maxTotalGradeAmountFilter == null
                    ? this.maxTotalGradeAmountFilterEmpty
                    : this.maxTotalGradeAmountFilter,
                this.minTotalGradeAmountFilter == null
                    ? this.minTotalGradeAmountFilterEmpty
                    : this.minTotalGradeAmountFilter,
                this.maxInsuranceAmountFilter == null
                    ? this.maxInsuranceAmountFilterEmpty
                    : this.maxInsuranceAmountFilter,
                this.minInsuranceAmountFilter == null
                    ? this.minInsuranceAmountFilterEmpty
                    : this.minInsuranceAmountFilter,
                this.maxTotalSalaryFilter == null ? this.maxTotalSalaryFilterEmpty : this.maxTotalSalaryFilter,
                this.minTotalSalaryFilter == null ? this.minTotalSalaryFilterEmpty : this.minTotalSalaryFilter,
                this.maxDearnessAllowanceFilter == null
                    ? this.maxDearnessAllowanceFilterEmpty
                    : this.maxDearnessAllowanceFilter,
                this.minDearnessAllowanceFilter == null
                    ? this.minDearnessAllowanceFilterEmpty
                    : this.minDearnessAllowanceFilter,
                this.maxTotalWithAllowanceFilter == null
                    ? this.maxTotalWithAllowanceFilterEmpty
                    : this.maxTotalWithAllowanceFilter,
                this.minTotalWithAllowanceFilter == null
                    ? this.minTotalWithAllowanceFilterEmpty
                    : this.minTotalWithAllowanceFilter,
                this.maxGovernmentAmountFilter == null
                    ? this.maxGovernmentAmountFilterEmpty
                    : this.maxGovernmentAmountFilter,
                this.minGovernmentAmountFilter == null
                    ? this.minGovernmentAmountFilterEmpty
                    : this.minGovernmentAmountFilter,
                this.maxInternalAmountFilter == null ? this.maxInternalAmountFilterEmpty : this.maxInternalAmountFilter,
                this.minInternalAmountFilter == null ? this.minInternalAmountFilterEmpty : this.minInternalAmountFilter,
                this.maxPaidSalaryAmountFilter == null
                    ? this.maxPaidSalaryAmountFilterEmpty
                    : this.maxPaidSalaryAmountFilter,
                this.minPaidSalaryAmountFilter == null
                    ? this.minPaidSalaryAmountFilterEmpty
                    : this.minPaidSalaryAmountFilter,
                this.schoolInfoNameFilter,
                this.employeeNameFilter,
                this.employeeLevelNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.monthFilter = -1;
        this.maxTechnicalAmountFilter = this.maxTechnicalAmountFilterEmpty;
        this.minTechnicalAmountFilter = this.maxTechnicalAmountFilterEmpty;
        this.maxTotalGradeAmountFilter = this.maxTotalGradeAmountFilterEmpty;
        this.minTotalGradeAmountFilter = this.maxTotalGradeAmountFilterEmpty;
        this.maxInsuranceAmountFilter = this.maxInsuranceAmountFilterEmpty;
        this.minInsuranceAmountFilter = this.maxInsuranceAmountFilterEmpty;
        this.maxTotalSalaryFilter = this.maxTotalSalaryFilterEmpty;
        this.minTotalSalaryFilter = this.maxTotalSalaryFilterEmpty;
        this.maxDearnessAllowanceFilter = this.maxDearnessAllowanceFilterEmpty;
        this.minDearnessAllowanceFilter = this.maxDearnessAllowanceFilterEmpty;
        this.maxTotalWithAllowanceFilter = this.maxTotalWithAllowanceFilterEmpty;
        this.minTotalWithAllowanceFilter = this.maxTotalWithAllowanceFilterEmpty;
        this.maxGovernmentAmountFilter = this.maxGovernmentAmountFilterEmpty;
        this.minGovernmentAmountFilter = this.maxGovernmentAmountFilterEmpty;
        this.maxInternalAmountFilter = this.maxInternalAmountFilterEmpty;
        this.minInternalAmountFilter = this.maxInternalAmountFilterEmpty;
        this.maxPaidSalaryAmountFilter = this.maxPaidSalaryAmountFilterEmpty;
        this.minPaidSalaryAmountFilter = this.maxPaidSalaryAmountFilterEmpty;
        this.schoolInfoNameFilter = '';
        this.employeeNameFilter = '';
        this.employeeLevelNameFilter = '';

        this.getEmployeeSalary();
    }
}
