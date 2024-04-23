import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InternalGradeSetupComponent } from './internalGradeSetup.component';

const routes: Routes = [
    {
        path: '',
        component: InternalGradeSetupComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InternalGradeSetupRoutingModule {}
