import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CreateEmployeeSalaryNewDto, CreateGenerateSalaryNewDto, CreateSalaryNewDto, EmployeeSalarySchoolInfoLookupTableDto, EmployeeSalaryServiceProxy, Months } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
    selector: 'app-create-generateEmployeeSalary',
    templateUrl: './create-generateEmployeeSalary.component.html'
})
export class CreateGenerateEmployeeSalaryComponent extends AppComponentBase implements OnInit {

    form: FormGroup;
    schools: EmployeeSalarySchoolInfoLookupTableDto[];
    data: CreateEmployeeSalaryNewDto[] = [];
    total: CreateEmployeeSalaryNewDto = new CreateEmployeeSalaryNewDto;
    createData: CreateSalaryNewDto;
    input: CreateGenerateSalaryNewDto;

    allMonths = [
        { id: 1, displayName: 'Baisakh' },
        { id: 2, displayName: 'Jestha' },
        { id: 3, displayName: 'Asar' },
        { id: 4, displayName: 'Shrawan' },
        { id: 5, displayName: 'Bhadra' },
        { id: 6, displayName: 'Asoj' },
        { id: 7, displayName: 'Kartik' },
        { id: 8, displayName: 'Mangsir' },
        { id: 9, displayName: 'Poush' },
        { id: 10, displayName: 'Magh' },
        { id: 11, displayName: 'Falgun' },
        { id: 12, displayName: 'Chaitra' },
    ];

    constructor(
        injector: Injector,
        private employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private fb: FormBuilder,
        private _fileDownloadService: FileDownloadService,
    ) { super(injector); }

    ngOnInit(): void {
        this.CreateForm();
        this.GetSchools();
    }

    CreateForm(item: any = {}) {
        this.form = this.fb.group({
            schoolIds: [item.schoolIds ? item.schoolIds : ''],
            year: [item.year ? item.year : 2081],
            months: []
        })
    }

    GetSchools() {
        this.employeeSalaryServiceProxy.getAllSchoolInfoForTableDropdown().subscribe((res) => {
            this.schools = res;
        })
    }

    GetData() {
        var schoolIds = this.form.get('schoolIds').value;
        var month = this.form.get('months').value;
        var year = this.form.get('year').value;
        this.input = new CreateGenerateSalaryNewDto();
        this.input.months = month;
        this.input.year = year;
        this.input.schoolIds = schoolIds;
        this.employeeSalaryServiceProxy.generateSalaryNew(this.input).subscribe((res) => {
            this.data = res.details;
            this.total = res.total;
        })
    }

    GetExcel() {
        var schoolIds = this.form.get('schoolIds').value;
        var month = this.form.get('months').value;
        var year = this.form.get('year').value;
        this.input = new CreateGenerateSalaryNewDto();
        this.input.months = month;
        this.input.year = year;
        this.input.schoolIds = schoolIds;
        this.employeeSalaryServiceProxy.generateSalaryNewExcel(this.input)
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    Remove(i: number) {
        const control = this.data;
        if (control.length > 1) {
            control.splice(i, 1);
        }
    }

    Create() {
        //    var schoolI = this.form.get('schoolIds').value;
        var month = this.form.get('months').value;
        this.createData = new CreateSalaryNewDto();
        this.createData.data = this.data;
        this.createData.months = month;
        this.createData.year = 2081;
        //  this.createData.schoolIds = schoolI;
        this.employeeSalaryServiceProxy.createSalaryNew(this.createData).subscribe();
        this.CreateForm();
        this.data = [];
    }
}