import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {FestivalBonusSettingRoutingModule} from './festivalBonusSetting-routing.module';
import {FestivalBonusSettingsComponent} from './festivalBonusSettings.component';
import {CreateOrEditFestivalBonusSettingModalComponent} from './create-or-edit-festivalBonusSetting-modal.component';
import {ViewFestivalBonusSettingModalComponent} from './view-festivalBonusSetting-modal.component';



@NgModule({
    declarations: [
        FestivalBonusSettingsComponent,
        CreateOrEditFestivalBonusSettingModalComponent,
        ViewFestivalBonusSettingModalComponent,
        
    ],
    imports: [AppSharedModule, FestivalBonusSettingRoutingModule , AdminSharedModule ],
    
})
export class FestivalBonusSettingModule {
}
