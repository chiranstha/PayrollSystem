import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EmployeeSalaryRoutingModule } from './employeeSalary-routing.module';
import { EmployeeSalaryComponent } from './employeeSalary.component';
import { CreateOrEditEmployeeSalaryComponent } from './create-or-edit-employeeSalary.component';
import { ViewEmployeeSalaryComponent } from './view-employeeSalary.component';

@NgModule({
    declarations: [EmployeeSalaryComponent, CreateOrEditEmployeeSalaryComponent, ViewEmployeeSalaryComponent],
    imports: [AppSharedModule, EmployeeSalaryRoutingModule, AdminSharedModule],
})
export class EmployeeSalaryModule {}
