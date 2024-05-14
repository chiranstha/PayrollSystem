import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { PrincipalAllowanceSettingsServiceProxy, CreateOrEditPrincipalAllowanceSettingDto ,PrincipalAllowanceSettingEmployeeLevelLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditPrincipalAllowanceSettingModal',
    templateUrl: './create-or-edit-PrincipalAllowanceSetting-modal.component.html'
})
export class CreateOrEditPrincipalAllowanceSettingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    PrincipalAllowanceSetting: CreateOrEditPrincipalAllowanceSettingDto = new CreateOrEditPrincipalAllowanceSettingDto();

    employeeLevelName = '';

	allEmployeeLevels: PrincipalAllowanceSettingEmployeeLevelLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _PrincipalAllowanceSettingsServiceProxy: PrincipalAllowanceSettingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(PrincipalAllowanceSettingId?: string): void {
    

        if (!PrincipalAllowanceSettingId) {
            this.PrincipalAllowanceSetting = new CreateOrEditPrincipalAllowanceSettingDto();
            this.PrincipalAllowanceSetting.id = PrincipalAllowanceSettingId;
            this.employeeLevelName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._PrincipalAllowanceSettingsServiceProxy.getPrincipalAllowanceSettingForEdit(PrincipalAllowanceSettingId).subscribe(result => {
                this.PrincipalAllowanceSetting = result;

                this.employeeLevelName = result.employeeLevelName;


                this.active = true;
                this.modal.show();
            });
        }
        this._PrincipalAllowanceSettingsServiceProxy.getAllEmployeeLevelForTableDropdown().subscribe(result => {						
						this.allEmployeeLevels = result;
					});
					
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._PrincipalAllowanceSettingsServiceProxy.createOrEdit(this.PrincipalAllowanceSetting)
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
