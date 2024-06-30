import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CategorySalaryServiceProxy,
    CategorySalaryEmployeeLevelLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'createOrEditCategorySalaryModal',
    templateUrl: './create-or-edit-categorySalary-modal.component.html',
})
export class CreateOrEditCategorySalaryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    form: FormGroup;
    active = false;
    saving = false;
    id: string;

    allCategories = [
        { id: 0, displayName: "Permanent" },
        { id: 1, displayName: "Temporary" },
        { id: 2, displayName: "ChildDevelopment" },
        { id: 3, displayName: "Donation" },
        { id: 4, displayName: "Helper" },
        { id: 5, displayName: "Contract" }
    ]
    allEmployeeLevels: CategorySalaryEmployeeLevelLookupTableDto[];
    constructor(
        injector: Injector,
        private _categorySalaryServiceProxy: CategorySalaryServiceProxy,
        private _dateTimeService: DateTimeService,
        private fb: FormBuilder,
    ) {
        super(injector);
    }

    createForm(item: any = {}) {
        this.form = this.fb.group({

            salary: [item.salary || '', Validators.required],
            category: [item.category || '', Validators.required],
            technicalAmount: [item.technicalAmount || '', Validators.required],
            employeeLevelId: [item.employeeLevelId || '', Validators.required],
            id: [item.id || null],
        });
    }

    show(categorySalaryId?: string): void {
        if (categorySalaryId) {
            this.id = categorySalaryId;
            this._categorySalaryServiceProxy.getCategorySalaryForEdit(categorySalaryId).subscribe((result) => {
                this.createForm(result);
            });
        }
        this.active = true;
        this.modal.show();
    }

    getAllEmployeeLevel() {
        this._categorySalaryServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe((result) => {
            this.allEmployeeLevels = result;
        });
    }

    save(): void {
        this.saving = true;
        this._categorySalaryServiceProxy
            .createOrEdit(this.form.getRawValue())
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this.getAllEmployeeLevel();
        this.createForm();
    }
}