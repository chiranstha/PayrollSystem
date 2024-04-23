import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SchoolInfosComponent } from './schoolInfos.component';

const routes: Routes = [
    {
        path: '',
        component: SchoolInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SchoolInfoRoutingModule {}
