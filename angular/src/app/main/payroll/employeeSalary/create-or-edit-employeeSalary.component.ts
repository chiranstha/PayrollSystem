import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    EmployeeSalaryServiceProxy,
    CreateOrEditEmployeeSalaryDto,
    EmployeeSalarySchoolInfoLookupTableDto,
    EmployeeSalaryEmployeeLookupTableDto,
    EmployeeSalaryEmployeeLevelLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-employeeSalary.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditEmployeeSalaryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    employeeSalary: CreateOrEditEmployeeSalaryDto = new CreateOrEditEmployeeSalaryDto();

    schoolInfoName = '';
    employeeName = '';
    employeeLevelName = '';

    allSchoolInfos: EmployeeSalarySchoolInfoLookupTableDto[];
    allEmployees: EmployeeSalaryEmployeeLookupTableDto[];
    allEmployeeLevels: EmployeeSalaryEmployeeLevelLookupTableDto[];

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('EmployeeSalary'), '/app/main/payroll/employeeSalary'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(employeeSalaryId?: string): void {
        if (!employeeSalaryId) {
            this.employeeSalary = new CreateOrEditEmployeeSalaryDto();
            this.employeeSalary.id = employeeSalaryId;
            this.schoolInfoName = '';
            this.employeeName = '';
            this.employeeLevelName = '';

            this.active = true;
        } else {
            this._employeeSalaryServiceProxy.getEmployeeSalaryForEdit(employeeSalaryId).subscribe((result) => {
                this.employeeSalary = result;


                this.active = true;
            });
        }
        this._employeeSalaryServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((result) => {
            this.allSchoolInfos = result;
        });
        this._employeeSalaryServiceProxy.getAllEmployeeForTableDropdown().subscribe((result) => {
            this.allEmployees = result;
        });
        this._employeeSalaryServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
    }

    save(): void {
        this.saving = true;

        this._employeeSalaryServiceProxy
            .createOrEdit(this.employeeSalary)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/payroll/employeeSalary']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._employeeSalaryServiceProxy
            .createOrEdit(this.employeeSalary)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.employeeSalary = new CreateOrEditEmployeeSalaryDto();
            });
    }
}
