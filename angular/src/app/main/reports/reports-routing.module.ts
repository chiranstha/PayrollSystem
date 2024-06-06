import { RouterModule, Routes } from "@angular/router";
import { SalaryReportComponent } from "./salaryReport/salaryReport.component";
import { NgModule } from "@angular/core";
import { SchoolWiseReportComponent } from "./schoolWiseReport/schoolWiseReport.component";
import { LevelWiseReportComponent } from "./levelWiseReport/levelWiseReport.component";

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
    },
    {
        path: 'levelWiseReport',
        component: LevelWiseReportComponent,
        pathMatch: 'full'
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
}) export class ReportsRoutingModule { }