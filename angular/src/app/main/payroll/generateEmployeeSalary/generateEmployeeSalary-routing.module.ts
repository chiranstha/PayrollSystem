import { RouterModule, Routes } from "@angular/router";
import { CreateGenerateEmployeeSalaryComponent } from "./create-generateEmployeeSalary.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: CreateGenerateEmployeeSalaryComponent,
        pathMatch: 'full',
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GenerateEmployeeSalaryRoutingModule{}