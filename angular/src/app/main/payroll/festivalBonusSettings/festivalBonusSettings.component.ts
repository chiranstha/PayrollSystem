import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { FestivalBonusSettingsServiceProxy,  Months, PercentOrAmount } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditFestivalBonusSettingModalComponent } from './create-or-edit-festivalBonusSetting-modal.component';

import { ViewFestivalBonusSettingModalComponent } from './view-festivalBonusSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './festivalBonusSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class FestivalBonusSettingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditFestivalBonusSettingModal', { static: true }) createOrEditFestivalBonusSettingModal: CreateOrEditFestivalBonusSettingModalComponent;
    @ViewChild('viewFestivalBonusSettingModal', { static: true }) viewFestivalBonusSettingModal: ViewFestivalBonusSettingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    monthIdFilter = -1;
    remarksFilter = '';

    months = Months;
    percentOrAmount = PercentOrAmount;
    constructor(
        injector: Injector,
        private _festivalBonusSettingsServiceProxy: FestivalBonusSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getFestivalBonusSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._festivalBonusSettingsServiceProxy.getAll(
            this.filterText,
            this.monthIdFilter,
            this.remarksFilter,
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

    createFestivalBonusSetting(): void {
        this.createOrEditFestivalBonusSettingModal.show();        
    }


    deleteFestivalBonusSetting(festivalBonusSetting: any): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._festivalBonusSettingsServiceProxy.delete(festivalBonusSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._festivalBonusSettingsServiceProxy.getFestivalBonusSettingsToExcel(
        this.filterText,
            this.monthIdFilter,
            this.remarksFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.monthIdFilter = -1;
    this.remarksFilter = '';

        this.getFestivalBonusSettings();
    }
}
