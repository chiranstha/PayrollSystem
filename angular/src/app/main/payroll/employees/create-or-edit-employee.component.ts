import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    EmployeesServiceProxy,
    CreateOrEditEmployeeDto,
    EmployeeEmployeeLevelLookupTableDto,
    EmployeeSchoolInfoLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './create-or-edit-employee.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditEmployeeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    employee: CreateOrEditEmployeeDto = new CreateOrEditEmployeeDto();

    employeeLevelName = '';
    schoolInfoName = '';

    allEmployeeLevels: EmployeeEmployeeLevelLookupTableDto[];
    allSchoolInfos: EmployeeSchoolInfoLookupTableDto[];

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Employee'), '/app/main/payroll/employees'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeesServiceProxy: EmployeesServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(employeeId?: string): void {
        if (!employeeId) {
            this.employee = new CreateOrEditEmployeeDto();
            this.employee.id = employeeId;
            this.employeeLevelName = '';
            this.schoolInfoName = '';

            this.active = true;
        } else {
            this._employeesServiceProxy.getEmployeeForEdit(employeeId).subscribe((result) => {
                this.employee = result;

                this.active = true;
            });
        }
        this._employeesServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
        this._employeesServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((result) => {
            this.allSchoolInfos = result;
        });
    }

    save(): void {
        this.saving = true;

        this._employeesServiceProxy
            .createOrEdit(this.employee)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['/app/main/payroll/employees']);
            });
    }

    saveAndNew(): void {
        this.saving = true;

        this._employeesServiceProxy
            .createOrEdit(this.employee)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.employee = new CreateOrEditEmployeeDto();
            });
    }
}
