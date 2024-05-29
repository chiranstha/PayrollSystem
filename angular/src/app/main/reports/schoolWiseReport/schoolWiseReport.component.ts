import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalarySchoolInfoLookupTableDto, EmployeeSalaryServiceProxy, SchoolWiseReportDto } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'schoolWiseReport',
    templateUrl: './schoolWiseReport.component.html'
})
export class SchoolWiseReportComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    data: SchoolWiseReportDto[];
    schools: EmployeeSalarySchoolInfoLookupTableDto[];
    constructor(injector: Injector,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.CreateForm();
        this.GetAllSchools();
    }

    CreateForm(item: any = {}) {
        this.form = this.fb.group({
            year: [item.year ? item.year : 2081],
            schoolId: [item.schoolId ? item.schoolId : '']
        })
    }

    GetAllSchools() {
        this._employeeSalaryServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((res) => {
            this.schools = res;
            this.form.get('schoolId').setValue(res[0].id);
            this._employeeSalaryServiceProxy.schoolWiseReport(this.form.get('year').value, this.form.get('schoolId').value).subscribe((res) => {
                this.data = res;
            })
        })
    }

    GetReport() {
        this._employeeSalaryServiceProxy.schoolWiseReport(this.form.get('year').value, this.form.get('schoolId').value).subscribe((res) => {
            this.data = res;
        })
    }

    Excel() {
        this._employeeSalaryServiceProxy.schoolWiseReportExcel(this.form.get('year').value, this.form.get('schoolId').value)
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}