import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { EmployeeSalarySchoolInfoLookupTableDto, ReportsServiceProxy, SchoolWiseReportDto } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'topicWiseReport',
    templateUrl: './topicWiseReport.component.html'
})
export class TopicWiseReportComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    data: SchoolWiseReportDto[];
    schools: EmployeeSalarySchoolInfoLookupTableDto[];

    CategoryEnum = [
        { id: 1, displayName: 'Basic Salary' },
        { id: 3, displayName: 'Grade Amount' },
        { id: 4, displayName: 'TechnicalGradeAmount' },
        { id: 5, displayName: 'EPFAmount' },
        { id: 6, displayName: 'InsuranceAmount' },
        { id: 8, displayName: 'InflationAmount' },
        { id: 9, displayName: 'PrincipalAllowance' },
        { id: 10, displayName: 'FestivalAllowance' },
    ];

    constructor(injector: Injector,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
        private _proxy: ReportsServiceProxy
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
            schoolId: [item.schoolId ? item.schoolId : ''],
            category: [item.category ? item.category : 1]
        })
    }

    GetAllSchools() {
        this._proxy.getAllSchoolInfoForTableDropdown().subscribe((res) => {
            this.schools = res;
            this.form.get('schoolId').setValue(res[0].id);
            this._proxy.getCategoryWiseReport(this.form.get('year').value, this.form.get('schoolId').value, this.form.get('category').value).subscribe((res) => {
                this.data = res;
            })
        })
    }

    GetReport() {
        this._proxy.getCategoryWiseReport(this.form.get('year').value, this.form.get('schoolId').value, this.form.get('category').value).subscribe((res) => {
            this.data = res;
        })
    }
}