import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PrincipalAllowanceSettingsComponent} from './principalAllowanceSettings.component';



const routes: Routes = [
    {
        path: '',
        component: PrincipalAllowanceSettingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PrincipalAllowanceSettingRoutingModule {
}
