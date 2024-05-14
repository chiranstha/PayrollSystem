import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {PrincipalAllowanceSettingRoutingModule} from './principalAllowanceSetting-routing.module';
import {PrincipalAllowanceSettingsComponent} from './principalAllowanceSettings.component';
import {CreateOrEditPrincipalAllowanceSettingModalComponent} from './create-or-edit-principalAllowanceSetting-modal.component';
import {ViewPrincipalAllowanceSettingModalComponent} from './view-principalAllowanceSetting-modal.component';



@NgModule({
    declarations: [
        PrincipalAllowanceSettingsComponent,
        CreateOrEditPrincipalAllowanceSettingModalComponent,
        ViewPrincipalAllowanceSettingModalComponent,
        
    ],
    imports: [AppSharedModule, PrincipalAllowanceSettingRoutingModule , AdminSharedModule ],
    
})
export class PrincipalAllowanceSettingModule {
}
