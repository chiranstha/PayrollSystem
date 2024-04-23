import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SchoolInfoRoutingModule } from './schoolInfo-routing.module';
import { SchoolInfosComponent } from './schoolInfos.component';
import { CreateOrEditSchoolInfoModalComponent } from './create-or-edit-schoolInfo-modal.component';
import { ViewSchoolInfoModalComponent } from './view-schoolInfo-modal.component';

@NgModule({
    declarations: [SchoolInfosComponent, CreateOrEditSchoolInfoModalComponent, ViewSchoolInfoModalComponent],
    imports: [AppSharedModule, SchoolInfoRoutingModule, AdminSharedModule],
})
export class SchoolInfoModule {}
