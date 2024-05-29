import { RouterModule, Routes } from "@angular/router";
import { SalaryReportComponent } from "./salaryReport/salaryReport.component";
import { NgModule } from "@angular/core";
import { SchoolWiseReportComponent } from "./schoolWiseReport/schoolWiseReport.component";

const routes: Routes = [
    {
        path: 'salaryReport',
        component: SalaryReportComponent,
        pathMatch: 'full'
    },
    {
        path: 'schoolWiseReport',
        component: SchoolWiseReportComponent,
        pathMatch: 'full'
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
}) export class ReportsRoutingModule { }