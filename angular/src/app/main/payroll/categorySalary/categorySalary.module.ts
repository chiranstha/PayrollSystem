import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CategorySalaryRoutingModule } from './categorySalary-routing.module';
import { CategorySalaryComponent } from './categorySalary.component';
import { CreateOrEditCategorySalaryModalComponent } from './create-or-edit-categorySalary-modal.component';
import { ViewCategorySalaryModalComponent } from './view-categorySalary-modal.component';

@NgModule({
    declarations: [CategorySalaryComponent, CreateOrEditCategorySalaryModalComponent, ViewCategorySalaryModalComponent],
    imports: [AppSharedModule, CategorySalaryRoutingModule, AdminSharedModule],
})
export class CategorySalaryModule {}
