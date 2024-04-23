import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { InternalGradeSetupRoutingModule } from './internalGradeSetup-routing.module';
import { InternalGradeSetupComponent } from './internalGradeSetup.component';
import { CreateOrEditInternalGradeSetupModalComponent } from './create-or-edit-internalGradeSetup-modal.component';
import { ViewInternalGradeSetupModalComponent } from './view-internalGradeSetup-modal.component';

@NgModule({
    declarations: [
        InternalGradeSetupComponent,
        CreateOrEditInternalGradeSetupModalComponent,
        ViewInternalGradeSetupModalComponent,
    ],
    imports: [AppSharedModule, InternalGradeSetupRoutingModule, AdminSharedModule],
})
export class InternalGradeSetupModule {}
