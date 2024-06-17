import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MonthlyAllowancesComponent } from './monthlyAllowance.component';

const routes: Routes = [
    {
        path: '',
        component: MonthlyAllowancesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MonthlyAllowanceRoutingModule {}
