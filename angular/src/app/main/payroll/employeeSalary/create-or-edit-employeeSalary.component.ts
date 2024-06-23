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
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    templateUrl: './create-or-edit-employeeSalary.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditEmployeeSalaryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    form: FormGroup;
    

    allSchoolInfos: EmployeeSalarySchoolInfoLookupTableDto[];
    allEmployees: EmployeeSalaryEmployeeLookupTableDto[];
    allEmployeeLevels: EmployeeSalaryEmployeeLevelLookupTableDto[];

    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('EmployeeSalary'), '/app/main/payroll/employeeSalary'),
        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
    ];
    id: string;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        
    private fb: FormBuilder,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this.getAllSchoolInfoForTableDropdown();
        this.getAllEmployeeForTableDropdown();
        this.getAllEmployeeLevelForTableDropdown();

        this.createForm();
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({
            month: [item.month??null,Validators.required],
            dateMiti: [item.dateMiti??''],
            basicSalary: [item.basicSalary??0,Validators.required],
            gradeAmount: [item.gradeAmount??0,Validators.required],
            technicalAmount: [item.technicalAmount??0,Validators.required],
            totalGradeAmount: [item.totalGradeAmount??0,Validators.required],
            totalBasicSalary: [item.totalBasicSalary??0,Validators.required],
            insuranceAmount: [item.insuranceAmount??0,Validators.required],
            totalSalary: [item.totalSalary??0,Validators.required],
            dearnessAllowance: [item.dearnessAllowance??0,Validators.required],
            principalAllowance: [item.principalAllowance??0,Validators.required],
            totalWithAllowance: [item.totalWithAllowance??0,Validators.required],
            totalMonth: [item.totalMonth??0,Validators.required],
            totalSalaryAmount: [item.totalSalaryAmount??0,Validators.required],
            festiableAllowance: [item.festiableAllowance??0,Validators.required],
            governmentAmount: [item.governmentAmount??0,Validators.required],
            internalAmount: [item.internalAmount??0,Validators.required],
            paidSalaryAmount: [item.paidSalaryAmount??0,Validators.required],
            schoolInfoId: [item.schoolInfoId??'',Validators.required],
            employeeId: [item.employeeId??'',Validators.required],
            employeeLevelId: [item.employeeLevelId??'',Validators.required],
            id: [item.id??null],


            
        });
    }

    show(employeeSalaryId?: string): void {
        if (employeeSalaryId) {
            this.id=employeeSalaryId;
            this._employeeSalaryServiceProxy.getEmployeeSalaryForEdit(employeeSalaryId).subscribe((result) => {
                this.createForm(result);


                this.active = true;
            });
        }
       
        
    }
    getAllSchoolInfoForTableDropdown()
    {
        this._employeeSalaryServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((result) => {
            this.allSchoolInfos = result;
        });
    }
    getAllEmployeeForTableDropdown()
    {
        
        this._employeeSalaryServiceProxy.getAllEmployeeForTableDropdown().subscribe((result) => {
            this.allEmployees = result;
        });
    }
    getAllEmployeeLevelForTableDropdown()
    {
        this._employeeSalaryServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
    }

    save(): void {
        this.saving = true;

        this._employeeSalaryServiceProxy
            .createOrEdit(this.form.getRawValue())
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
