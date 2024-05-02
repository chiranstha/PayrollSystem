import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EmployeeRoutingModule } from './employee-routing.module';
import { EmployeesComponent } from './employees.component';
import { CreateOrEditEmployeeComponent } from './create-or-edit-employee.component';
import { ViewEmployeeComponent } from './view-employee.component';

@NgModule({
    declarations: [EmployeesComponent, CreateOrEditEmployeeComponent, ViewEmployeeComponent],
    imports: [AppSharedModule, EmployeeRoutingModule, AdminSharedModule],
})
export class EmployeeModule {}
