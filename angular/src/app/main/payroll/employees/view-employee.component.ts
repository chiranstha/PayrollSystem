import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    EmployeesServiceProxy,
    GetEmployeeForViewDto,
    EmployeeCategory,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-employee.component.html',
    animations: [appModuleAnimation()],
})
export class ViewEmployeeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    item: GetEmployeeForViewDto;
    employeeCategory = EmployeeCategory;

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Employee'), '/app/main/payroll/employees'),
        new BreadcrumbItem(this.l('Employees') + '' + this.l('Details')),
    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeesServiceProxy: EmployeesServiceProxy
    ) {
        super(injector);
        this.item = new GetEmployeeForViewDto();
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(employeeId: string): void {
        this._employeesServiceProxy.getEmployeeForView(employeeId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
    }
}
