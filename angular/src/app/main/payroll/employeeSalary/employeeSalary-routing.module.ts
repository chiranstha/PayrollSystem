import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeSalaryComponent } from './employeeSalary.component';
import { CreateOrEditEmployeeSalaryComponent } from './create-or-edit-employeeSalary.component';
import { ViewEmployeeSalaryComponent } from './view-employeeSalary.component';

const routes: Routes = [
    {
        path: '',
        component: EmployeeSalaryComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditEmployeeSalaryComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewEmployeeSalaryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeSalaryRoutingModule {}
