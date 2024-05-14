﻿import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { GradeUpgradesServiceProxy, EmployeeGrade } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGradeUpgradeModalComponent } from './create-or-edit-gradeUpgrade-modal.component';

import { ViewGradeUpgradeModalComponent } from './view-gradeUpgrade-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './gradeUpgrades.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GradeUpgradesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditGradeUpgradeModal', { static: true }) createOrEditGradeUpgradeModal: CreateOrEditGradeUpgradeModalComponent;
    @ViewChild('viewGradeUpgradeModal', { static: true }) viewGradeUpgradeModal: ViewGradeUpgradeModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    dateMitiFilter = '';
    gradeFilter = -1;
    remarksFilter = '';
        employeeNameFilter = '';

    employeeGrade = EmployeeGrade;





    constructor(
        injector: Injector,
        private _gradeUpgradesServiceProxy: GradeUpgradesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getGradeUpgrades(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._gradeUpgradesServiceProxy.getAll(
            this.filterText,
            this.dateMitiFilter,
            this.gradeFilter,
            this.remarksFilter,
            this.employeeNameFilter,
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

    createGradeUpgrade(): void {
        this.createOrEditGradeUpgradeModal.show();        
    }


    deleteGradeUpgrade(gradeUpgrade: any): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._gradeUpgradesServiceProxy.delete(gradeUpgrade.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._gradeUpgradesServiceProxy.getGradeUpgradesToExcel(
        this.filterText,
            this.dateMitiFilter,
            this.gradeFilter,
            this.remarksFilter,
            this.employeeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.dateMitiFilter = '';
    this.gradeFilter = -1;
    this.remarksFilter = '';
		this.employeeNameFilter = '';
					
        this.getGradeUpgrades();
    }
}
