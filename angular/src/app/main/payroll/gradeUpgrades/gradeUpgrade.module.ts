import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {GradeUpgradeRoutingModule} from './gradeUpgrade-routing.module';
import {GradeUpgradesComponent} from './gradeUpgrades.component';
import {CreateOrEditGradeUpgradeModalComponent} from './create-or-edit-gradeUpgrade-modal.component';
import {ViewGradeUpgradeModalComponent} from './view-gradeUpgrade-modal.component';



@NgModule({
    declarations: [
        GradeUpgradesComponent,
        CreateOrEditGradeUpgradeModalComponent,
        ViewGradeUpgradeModalComponent,
        
    ],
    imports: [AppSharedModule, GradeUpgradeRoutingModule , AdminSharedModule ],
    
})
export class GradeUpgradeModule {
}
