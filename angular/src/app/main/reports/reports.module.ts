import { NgModule } from "@angular/core";
import { SalaryReportComponent } from "./salaryReport/salaryReport.component";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { ReportsRoutingModule } from "./reports-routing.module";
import { AdminSharedModule } from "@app/admin/shared/admin-shared.module";
import { SchoolWiseReportComponent } from "./schoolWiseReport/schoolWiseReport.component";
import { CommonModule } from "@angular/common";
import { NgSelectModule } from "@ng-select/ng-select";
import { LevelWiseReportComponent } from "./levelWiseReport/levelWiseReport.component";
import { TopicWiseReportComponent } from "./topicWiseReport/topicWiseReport.component";
import { PhaseReportComponent } from "./phaseWiseReport/phaseReport.component";
import { TeacherReportComponent } from "./teacherReport/teacherReport.component";

@NgModule({
    declarations: [SalaryReportComponent, SchoolWiseReportComponent, LevelWiseReportComponent,TopicWiseReportComponent,PhaseReportComponent,TeacherReportComponent],
    imports: [AppSharedModule, ReportsRoutingModule, AdminSharedModule, NgSelectModule]
}) export class ReportsModule { } 