import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalaryEmployeeLevelLookupTableDto, EmployeeSalaryServiceProxy, MonthwiseReportDto, ReportsServiceProxy, TeacherWiseReportDto } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'teacherReport',
    templateUrl: './teacherReport.component.html'
})

export class TeacherReportComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    data: TeacherWiseReportDto[];
    employees: EmployeeSalaryEmployeeLevelLookupTableDto[];

    constructor(injector: Injector,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
        private _employeeTeacherServiceProxy: ReportsServiceProxy
    ) { super(injector); }

    ngOnInit(): void {
        this.CreateForm();
        this.GetAllEmployees();
    }

    CreateForm(item: any = {}) {
        this.form = this.fb.group({
            year: [item.year ? item.year : 2081],
            employeeId: [item.employeeId ? item.employeeId : '']
        })
    }

    GetSalaries() {
        this._employeeTeacherServiceProxy.teacherWiseReport(this.form.get('year').value,this.form.get('employeeId').value).subscribe((res) => {
            this.data = res;
        })
    }

    GetAllEmployees() {
        this._employeeTeacherServiceProxy.getAllEmployee().subscribe((res) => {
            this.employees = res;
            this.form.get('employeeId').setValue(res[0].id);
            this._employeeTeacherServiceProxy.teacherWiseReport(this.form.get('year').value,this.form.get('employeeId').value).subscribe((res) => {
                this.data = res;
            })
        })
    }

    // Excel(){
    //     this._employeeTeacherServiceProxy.getAllSalariesExcel(this.form.get('year').value)
    //     .subscribe(result => {
    //         this._fileDownloadService.downloadTempFile(result);
    //     });
    // }
   
}