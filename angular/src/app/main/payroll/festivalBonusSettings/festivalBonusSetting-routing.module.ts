import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {FestivalBonusSettingsComponent} from './festivalBonusSettings.component';



const routes: Routes = [
    {
        path: '',
        component: FestivalBonusSettingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FestivalBonusSettingRoutingModule {
}
