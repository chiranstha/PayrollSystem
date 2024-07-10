import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalaryServiceProxy, MonthwiseReportDto, PhaseWiseReportDto, ReportsServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'phaseReport',
    templateUrl: './phaseReport.component.html'
})

export class PhaseReportComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    data: PhaseWiseReportDto[];

    constructor(injector: Injector,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
        private _employeePhaseServiceProxy: ReportsServiceProxy
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
        this._employeePhaseServiceProxy.phaseWiseReport(this.form.get('year').value).subscribe((res) => {
            this.data = res;
        })
    }

    // Excel(){
    //     this._employeePhaseServiceProxy.getAllSalariesExcel(this.form.get('year').value)
    //     .subscribe(result => {
    //         this._fileDownloadService.downloadTempFile(result);
    //     });
    // }
   
}