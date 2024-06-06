import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalaryEmployeeLevelLookupTableDto, LevelWiseReportDto, ReportsServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'levelWiseReport',
    templateUrl: './levelWiseReport.component.html'
})
export class LevelWiseReportComponent extends AppComponentBase implements OnInit {
    form: FormGroup;
    levels: EmployeeSalaryEmployeeLevelLookupTableDto[];

    data: LevelWiseReportDto[];
    constructor(injector: Injector,
        private fb: FormBuilder,
        private _serviceProxy: ReportsServiceProxy,
        private _fileDownloadService: FileDownloadService) {
        super(injector);
    }

    ngOnInit(): void {
        this.CreateForm();
        this.GetAllLevels();
    }

    CreateForm(item: any = {}) {
        this.form = this.fb.group({
            year: [item.year ? item.year : 2081],
            level: [item.level ? item.level : '']
        })
    }

    GetReport() {
        this._serviceProxy.getLevelWiseReport(this.form.get('year').value, this.form.get('level').value).subscribe((res) => {
            this.data = res;
        })
    }

    GetAllLevels(){
        this._serviceProxy.getAllLevels().subscribe((res)=> {
            this.levels = res;
            this.form.get('level').setValue(res[0].id);
            this._serviceProxy.getLevelWiseReport(this.form.get('year').value, res[0].id).subscribe((res) => {
                this.data = res;
            })
        })
    }
}