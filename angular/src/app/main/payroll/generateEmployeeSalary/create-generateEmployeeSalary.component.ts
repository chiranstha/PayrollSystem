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
            remarks: [item.remarks ? item.remarks : ''],
            year: [item.year ? item.year : 2081],
            months: [],
            totalAmount: [item.totalAmount ? item.totalAmount : 0],
            dueAmount: [item.dueAmount ? item.dueAmount : 0],
            extraAmount: [item.extraAmount ? item.extraAmount : 0],
            finalAmount: [item.finalAmount ? item.finalAmount : 0]
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
            this.form.get('totalAmount').setValue(res.total.totalPaidAmount);
            this.form.get('finalAmount').setValue(res.total.totalPaidAmount);
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
        this.CalculateTotal();
    }

    CalculateTotal() {
        let basicSalary = 0;
        let epfAmount = 0;
        let festivalAllowance = 0;
        let gradeAmount = 0;
        let inflationAllowance = 0;
        let insuranceAmount = 0;
        let internalAmount = 0;
        let principalAllowance = 0;
        let technicalGradeAmount = 0;
        let total = 0;
        let totalForAllMonths = 0;
        let totalGradeAmount = 0;
        let totalPaidAmount = 0;
        let totalSalary = 0;
        let totalSalaryAmount = 0;
        let totalWithAllowanceForAllMonths = 0;
        for (let i = 0; i < this.data.length; i++) {
            basicSalary += this.data[i].basicSalary;
            epfAmount += this.data[i].epfAmount;
            festivalAllowance += this.data[i].festivalAllowance;
            gradeAmount += this.data[i].gradeAmount;
            inflationAllowance += this.data[i].inflationAllowance;
            insuranceAmount += this.data[i].insuranceAmount;
            internalAmount += this.data[i].internalAmount;
            principalAllowance += this.data[i].principalAllowance;
            technicalGradeAmount += this.data[i].technicalGradeAmount;
            total += this.data[i].total;
            totalForAllMonths += this.data[i].totalForAllMonths;
            totalGradeAmount += this.data[i].totalGradeAmount;
            totalPaidAmount += this.data[i].totalPaidAmount;
            totalSalary += this.data[i].totalSalary;
            totalSalaryAmount += this.data[i].totalSalaryAmount;
            totalWithAllowanceForAllMonths += this.data[i].totalWithAllowanceForAllMonths;
        }
        this.total.basicSalary = basicSalary;
        this.total.epfAmount = epfAmount;
        this.total.festivalAllowance = festivalAllowance;
        this.total.gradeAmount = gradeAmount;
        this.total.inflationAllowance = inflationAllowance;
        this.total.insuranceAmount = insuranceAmount;
        this.total.internalAmount = internalAmount;
        this.total.principalAllowance =  principalAllowance;
        this.total.technicalGradeAmount = technicalGradeAmount;
        this.total.total = total;
        this.total.totalForAllMonths = totalForAllMonths;
        this.total.totalGradeAmount = totalGradeAmount;
        this.total.totalPaidAmount = totalPaidAmount;
        this.total.totalSalary = totalSalary;
        this.total.totalSalaryAmount = totalSalaryAmount;
        this.total.totalWithAllowanceForAllMonths = totalWithAllowanceForAllMonths;        
        this.form.get('totalAmount').setValue(totalPaidAmount);
        this.finalChange();
    }

    Create() {
        var month = this.form.get('months').value;
        this.createData = new CreateSalaryNewDto();
        this.createData.data = this.data;
        this.createData.months = month;
        this.createData.year = this.form.get('year').value;
        var totalAmount = this.form.get('totalAmount').value;
        var dueAmount = this.form.get('dueAmount').value;
        var extraAmount = this.form.get('extraAmount').value;
        var finalAmount = this.form.get('finalAmount').value;
        var remarks = this.form.get('remarks').value;
        this.createData.totalAmount = totalAmount;
        this.createData.dueAmount = dueAmount;
        this.createData.extraAmount = extraAmount;
        this.createData.finalAmount = finalAmount;
        this.createData.remarks = remarks;
        this.employeeSalaryServiceProxy.createSalaryNew(this.createData).subscribe();
        this.CreateForm();
        this.data = [];
    }

    finalChange() {
        var finalAmount = this.form.get('totalAmount').value - this.form.get('dueAmount').value + this.form.get('extraAmount').value;
        const abc = finalAmount.toFixed(2);
        this.form.get('finalAmount').setValue(abc);
    }
}