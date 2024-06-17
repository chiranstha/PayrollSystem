﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SchoolLevelsComponent } from './employeeLevels.component';

const routes: Routes = [
    {
        path: '',
        component: SchoolLevelsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SchoolLevelRoutingModule {}
