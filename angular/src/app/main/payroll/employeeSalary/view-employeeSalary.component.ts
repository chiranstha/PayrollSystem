import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    EmployeeSalaryServiceProxy,
    GetEmployeeSalaryForViewDto,
    Months,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-employeeSalary.component.html',
    animations: [appModuleAnimation()],
})
export class ViewEmployeeSalaryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetEmployeeSalaryForViewDto;
    months = Months;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('EmployeeSalary'), '/app/main/payroll/employeeSalary'),
        new BreadcrumbItem(this.l('EmployeeSalary') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy
    ) {
        super(injector);
        this.item = new GetEmployeeSalaryForViewDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(employeeSalaryId: string): void {
        this._employeeSalaryServiceProxy.getEmployeeSalaryForView(employeeSalaryId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}
