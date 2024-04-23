import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategorySalaryComponent } from './categorySalary.component';

const routes: Routes = [
    {
        path: '',
        component: CategorySalaryComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CategorySalaryRoutingModule {}
