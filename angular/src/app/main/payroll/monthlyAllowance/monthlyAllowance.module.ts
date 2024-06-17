import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MonthlyAllowanceRoutingModule } from './monthlyAllowance-routing.module';
import { MonthlyAllowancesComponent } from './monthlyAllowance.component';
import { CreateOrEditMonthlyAllowanceModalComponent } from './create-or-edit-monthlyAllowance-modal.component';
import { ViewMonthlyAllowanceModalComponent } from './view-monthlyAllowance-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [MonthlyAllowancesComponent, CreateOrEditMonthlyAllowanceModalComponent, ViewMonthlyAllowanceModalComponent],
    imports: [AppSharedModule, MonthlyAllowanceRoutingModule,ReactiveFormsModule, FormsModule, AdminSharedModule],
})
export class MonthlyAllowanceModule {}
