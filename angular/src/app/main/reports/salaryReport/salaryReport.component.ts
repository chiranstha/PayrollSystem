import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalaryServiceProxy, MonthwiseReportDto } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'salaryReport',
    templateUrl: './salaryReport.component.html'
})

export class SalaryReportComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    data: MonthwiseReportDto[];

    constructor(injector: Injector,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy
    ) { super(injector); }

    ngOnInit(): void {
        this.CreateForm();
        this.GetSalaries();
    }

    CreateForm(item: any = {}) {
        this.form = this.fb.group({
            year: [item.year ? item.year : 2081]
        })
    }

    GetSalaries() {
        this._employeeSalaryServiceProxy.getAllSalaries(this.form.get('year').value).subscribe((res) => {
            this.data = res;
        })
    }

    Excel(){
        this._employeeSalaryServiceProxy.getAllSalariesExcel(this.form.get('year').value)
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
        });
    }
   
}