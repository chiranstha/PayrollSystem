import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { GradeUpgradesServiceProxy, CreateOrEditGradeUpgradeDto ,GradeUpgradeEmployeeLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditGradeUpgradeModal',
    templateUrl: './create-or-edit-gradeUpgrade-modal.component.html'
})
export class CreateOrEditGradeUpgradeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    gradeUpgrade: CreateOrEditGradeUpgradeDto = new CreateOrEditGradeUpgradeDto();

    employeeName = '';

	allEmployees: GradeUpgradeEmployeeLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _gradeUpgradesServiceProxy: GradeUpgradesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(gradeUpgradeId?: string): void {
    

        if (!gradeUpgradeId) {
            this.gradeUpgrade = new CreateOrEditGradeUpgradeDto();
            this.gradeUpgrade.id = gradeUpgradeId;
            this.employeeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._gradeUpgradesServiceProxy.getGradeUpgradeForEdit(gradeUpgradeId).subscribe(result => {
                this.gradeUpgrade = result;

                this.employeeName = result.employeeName;


                this.active = true;
                this.modal.show();
            });
        }
        this._gradeUpgradesServiceProxy.getAllEmployeeForTableDropdown().subscribe(result => {						
						this.allEmployees = result;
					});
					
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._gradeUpgradesServiceProxy.createOrEdit(this.gradeUpgrade)
             .pipe(finalize(() => { this.saving = false;}))
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
        
     }    
}
