import { AppSharedModule } from "@app/shared/app-shared.module";
import { CreateGenerateEmployeeSalaryComponent } from "./create-generateEmployeeSalary.component";
import { NgModule } from "@angular/core";
import { GenerateEmployeeSalaryRoutingModule } from "./generateEmployeeSalary-routing.module";
import { AdminSharedModule } from "@app/admin/shared/admin-shared.module";
import { NgSelectModule } from '@ng-select/ng-select';
@NgModule({
    declarations: [CreateGenerateEmployeeSalaryComponent],
    imports: [AppSharedModule, GenerateEmployeeSalaryRoutingModule, AdminSharedModule,NgSelectModule]
})
export class GenerateEmployeeSalaryModule {}