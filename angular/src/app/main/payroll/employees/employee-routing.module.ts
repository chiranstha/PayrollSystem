import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeesComponent } from './employees.component';
import { CreateOrEditEmployeeComponent } from './create-or-edit-employee.component';
import { ViewEmployeeComponent } from './view-employee.component';

const routes: Routes = [
    {
        path: '',
        component: EmployeesComponent,
        pathMatch: 'full',
    },

    {
        path: 'createOrEdit',
        component: CreateOrEditEmployeeComponent,
        pathMatch: 'full',
    },

    {
        path: 'view',
        component: ViewEmployeeComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeRoutingModule {}
