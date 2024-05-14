import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GradeUpgradesComponent} from './gradeUpgrades.component';



const routes: Routes = [
    {
        path: '',
        component: GradeUpgradesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GradeUpgradeRoutingModule {
}
