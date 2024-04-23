import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'payroll/schoolInfos',
                        loadChildren: () => import('./payroll/schoolInfos/schoolInfo.module').then(m => m.SchoolInfoModule),
                        data: { permission: 'Pages.SchoolInfos' }
                    },
                
                    
                    {
                        path: 'payroll/internalGradeSetup',
                        loadChildren: () => import('./payroll/internalGradeSetup/internalGradeSetup.module').then(m => m.InternalGradeSetupModule),
                        data: { permission: 'Pages.InternalGradeSetup' }
                    },
                
                    
                    {
                        path: 'payroll/categorySalary',
                        loadChildren: () => import('./payroll/categorySalary/categorySalary.module').then(m => m.CategorySalaryModule),
                        data: { permission: 'Pages.CategorySalary' }
                    },
                
                    
                    {
                        path: 'payroll/employeeLevels',
                        loadChildren: () => import('./payroll/employeeLevels/employeeLevel.module').then(m => m.EmployeeLevelModule),
                        data: { permission: 'Pages.EmployeeLevels' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
