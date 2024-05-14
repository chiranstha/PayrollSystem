import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'payroll/gradeUpgrades',
                        loadChildren: () => import('./payroll/gradeUpgrades/gradeUpgrade.module').then(m => m.GradeUpgradeModule),
                        data: { permission: 'Pages.GradeUpgrades' }
                    },
                
                    
                    {
                        path: 'payroll/festivalBonusSettings',
                        loadChildren: () => import('./payroll/festivalBonusSettings/festivalBonusSetting.module').then(m => m.FestivalBonusSettingModule),
                        data: { permission: 'Pages.FestivalBonusSettings' }
                    },
                
                    
                    {
                        path: 'payroll/tbl_PrincipalAllowanceSettings',
                        loadChildren: () => import('./payroll/PrincipalAllowanceSettings/principalAllowanceSetting.module').then(m => m.PrincipalAllowanceSettingModule),
                        data: { permission: 'Pages.PrincipalAllowanceSettings' }
                    },
                
                    
                    {
                        path: 'payroll/employeeSalary',
                        loadChildren: () => import('./payroll/employeeSalary/employeeSalary.module').then(m => m.EmployeeSalaryModule),
                        data: { permission: 'Pages.EmployeeSalary' }
                    },
                
                    
                    {
                        path: 'payroll/employees',
                        loadChildren: () => import('./payroll/employees/employee.module').then(m => m.EmployeeModule),
                        data: { permission: 'Pages.Employees' }
                    },
                
                    
                    {
                        path: 'master/financialYears',
                        loadChildren: () => import('./master/financialYears/financialYear.module').then(m => m.FinancialYearModule),
                        data: { permission: 'Pages.FinancialYears' }
                    },
                
                    
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
