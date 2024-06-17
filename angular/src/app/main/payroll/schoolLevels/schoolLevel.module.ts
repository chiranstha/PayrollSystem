import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SchoolLevelRoutingModule } from './schoolLevel-routing.module';
import { CreateOrEditSchoolLevelModalComponent } from './create-or-edit-schoolLevel-modal.component';
import { ViewSchoolLevelModalComponent } from './view-schoolLevel-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SchoolLevelsComponent } from './employeeLevels.component';

@NgModule({
    declarations: [SchoolLevelsComponent, CreateOrEditSchoolLevelModalComponent, ViewSchoolLevelModalComponent],
    imports: [AppSharedModule, SchoolLevelRoutingModule,ReactiveFormsModule, FormsModule, AdminSharedModule],
})
export class SchoolLevelModule {}
