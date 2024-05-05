import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EmployeeLevelRoutingModule } from './employeeLevel-routing.module';
import { EmployeeLevelsComponent } from './employeeLevels.component';
import { CreateOrEditEmployeeLevelModalComponent } from './create-or-edit-employeeLevel-modal.component';
import { ViewEmployeeLevelModalComponent } from './view-employeeLevel-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [EmployeeLevelsComponent, CreateOrEditEmployeeLevelModalComponent, ViewEmployeeLevelModalComponent],
    imports: [AppSharedModule, EmployeeLevelRoutingModule,ReactiveFormsModule, FormsModule, AdminSharedModule],
})
export class EmployeeLevelModule {}
