import { RouterModule, Routes } from "@angular/router";
import { SalaryReportComponent } from "./salaryReport/salaryReport.component";
import { NgModule } from "@angular/core";
import { SchoolWiseReportComponent } from "./schoolWiseReport/schoolWiseReport.component";
import { LevelWiseReportComponent } from "./levelWiseReport/levelWiseReport.component";
import { TopicWiseReportComponent } from "./topicWiseReport/topicWiseReport.component";
import { PhaseReportComponent } from "./phaseWiseReport/phaseReport.component";
import { TeacherReportComponent } from "./teacherReport/teacherReport.component";

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
    },
    {
        path: 'topicWiseReport',
        component: TopicWiseReportComponent,
        pathMatch: 'full'
    },
    {
        path: 'phaseReport',
        component: PhaseReportComponent,
        pathMatch: 'full'
    },
    {
        path: 'teacherReport',
        component: TeacherReportComponent,
        pathMatch: 'full'
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
}) export class ReportsRoutingModule { }