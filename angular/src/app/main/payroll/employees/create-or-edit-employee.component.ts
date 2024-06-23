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
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    templateUrl: './create-or-edit-employee.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditEmployeeComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    form: FormGroup;
   
    

    allEmployeeLevels: EmployeeEmployeeLevelLookupTableDto[];
    allSchoolInfos: EmployeeSchoolInfoLookupTableDto[];

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Employee'), '/app/main/payroll/employees'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];
    id: string;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeesServiceProxy: EmployeesServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this.getAllEmployeeLevelDropdown();
        this.getAllSchoolInfoForTableDropdown();
        this.createForm();
    }


    createForm(item: any = {}) {
        this.form = this.fb.group({
          
            category: [item.category??""],
            providentFund: [item.providentFund??""],
            panNo: [item.panNo??""],
            insuranceNo: [item.insuranceNo??""],
            name: [item.name??""],
            bankName: [item.bankName??""],
            bankAccountNo: [item.bankAccountNo??""],
            pansionMiti: [item.pansionMiti],
            dateOfJoinMiti: [item.dateOfJoinMiti],
            insuranceAmount: [item.insuranceAmount??0],
            isDearnessAllowance: [item.isDearnessAllowance??false],
            addEPF: [item.addEPF??false],
            isPrincipal: [item.isPrincipal??false],
            isGovernment: [item.isGovernment??false],
            isInternal: [item.isInternal??false],
            isTechnical: [item.isTechnical??false],
            grade: [item.grade],
            technicalGrade: [item.technicalGrade],
            employeeLevelId: [item.employeeLevelId],
            schoolInfoId: [item.schoolInfoId],
            id: [item.id]




        
            
        });
    }

    show(employeeId?: string): void {
        if (employeeId) {
           this.id=employeeId;
           

       
            this._employeesServiceProxy.getEmployeeForEdit(employeeId).subscribe((result) => {
                this.createForm(result);

                this.active = true;
            });
        }
       
       
    }

    getAllEmployeeLevelDropdown()
    {
        this._employeesServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
    }

    getAllSchoolInfoForTableDropdown()
    { this._employeesServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((result) => {
        this.allSchoolInfos = result;
    });}

    save(): void {
        this.saving = true;

        this._employeesServiceProxy
            .createOrEdit(this.form.getRawValue())
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
            .createOrEdit(this.form.getRawValue())
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.form.reset();
            });
    }
}
