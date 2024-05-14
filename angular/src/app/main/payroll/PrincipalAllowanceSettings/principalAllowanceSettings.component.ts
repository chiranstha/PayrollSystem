import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PrincipalAllowanceSettingsServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPrincipalAllowanceSettingModalComponent } from './create-or-edit-principalAllowanceSetting-modal.component';

import { ViewPrincipalAllowanceSettingModalComponent } from './view-principalAllowanceSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './principalAllowanceSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PrincipalAllowanceSettingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditPrincipalAllowanceSettingModal', { static: true }) createOrEditPrincipalAllowanceSettingModal: CreateOrEditPrincipalAllowanceSettingModalComponent;
    @ViewChild('viewPrincipalAllowanceSettingModal', { static: true }) viewPrincipalAllowanceSettingModal: ViewPrincipalAllowanceSettingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxAmountFilter : number;
		maxAmountFilterEmpty : number;
		minAmountFilter : number;
		minAmountFilterEmpty : number;
        employeeLevelNameFilter = '';






    constructor(
        injector: Injector,
        private _tbl_PrincipalAllowanceSettingsServiceProxy: PrincipalAllowanceSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTbl_PrincipalAllowanceSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._tbl_PrincipalAllowanceSettingsServiceProxy.getAll(
            this.filterText,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.employeeLevelNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTbl_PrincipalAllowanceSetting(): void {
        this.createOrEditPrincipalAllowanceSettingModal.show();        
    }


    deleteTbl_PrincipalAllowanceSetting(tbl_PrincipalAllowanceSetting: any): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._tbl_PrincipalAllowanceSettingsServiceProxy.delete(tbl_PrincipalAllowanceSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._tbl_PrincipalAllowanceSettingsServiceProxy.getPrincipalAllowanceSettingsToExcel(
        this.filterText,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.employeeLevelNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxAmountFilter = this.maxAmountFilterEmpty;
		this.minAmountFilter = this.maxAmountFilterEmpty;
		this.employeeLevelNameFilter = '';
					
        this.getTbl_PrincipalAllowanceSettings();
    }
}
