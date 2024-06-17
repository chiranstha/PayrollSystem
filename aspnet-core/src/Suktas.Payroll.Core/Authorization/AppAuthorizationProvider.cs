using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Suktas.Payroll.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var gradeUpgrades = pages.CreateChildPermission(AppPermissions.Pages_GradeUpgrades, L("GradeUpgrades"));
            gradeUpgrades.CreateChildPermission(AppPermissions.Pages_GradeUpgrades_Create, L("CreateNewGradeUpgrade"));
            gradeUpgrades.CreateChildPermission(AppPermissions.Pages_GradeUpgrades_Edit, L("EditGradeUpgrade"));
            gradeUpgrades.CreateChildPermission(AppPermissions.Pages_GradeUpgrades_Delete, L("DeleteGradeUpgrade"));

            var festivalBonusSettings = pages.CreateChildPermission(AppPermissions.Pages_FestivalBonusSettings, L("FestivalBonusSettings"));
            festivalBonusSettings.CreateChildPermission(AppPermissions.Pages_FestivalBonusSettings_Create, L("CreateNewFestivalBonusSetting"));
            festivalBonusSettings.CreateChildPermission(AppPermissions.Pages_FestivalBonusSettings_Edit, L("EditFestivalBonusSetting"));
            festivalBonusSettings.CreateChildPermission(AppPermissions.Pages_FestivalBonusSettings_Delete, L("DeleteFestivalBonusSetting"));

            var tbl_PrincipalAllowanceSettings = pages.CreateChildPermission(AppPermissions.Pages_PrincipalAllowanceSettings, L("PrincipalAllowanceSettings"));
            tbl_PrincipalAllowanceSettings.CreateChildPermission(AppPermissions.Pages_PrincipalAllowanceSettings_Create, L("CreateNewPrincipalAllowanceSetting"));
            tbl_PrincipalAllowanceSettings.CreateChildPermission(AppPermissions.Pages_PrincipalAllowanceSettings_Edit, L("EditPrincipalAllowanceSetting"));
            tbl_PrincipalAllowanceSettings.CreateChildPermission(AppPermissions.Pages_PrincipalAllowanceSettings_Delete, L("DeletePrincipalAllowanceSetting"));

            var employeeSalary = pages.CreateChildPermission(AppPermissions.Pages_EmployeeSalary, L("EmployeeSalary"));
            employeeSalary.CreateChildPermission(AppPermissions.Pages_EmployeeSalary_Create, L("CreateNewEmployeeSalary"));
            employeeSalary.CreateChildPermission(AppPermissions.Pages_EmployeeSalary_Edit, L("EditEmployeeSalary"));
            employeeSalary.CreateChildPermission(AppPermissions.Pages_EmployeeSalary_Delete, L("DeleteEmployeeSalary"));

            var employees = pages.CreateChildPermission(AppPermissions.Pages_Employees, L("Employees"), multiTenancySides: MultiTenancySides.Tenant);
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Create, L("CreateNewEmployee"), multiTenancySides: MultiTenancySides.Tenant);
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Edit, L("EditEmployee"), multiTenancySides: MultiTenancySides.Tenant);
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Delete, L("DeleteEmployee"), multiTenancySides: MultiTenancySides.Tenant);

            var financialYears = pages.CreateChildPermission(AppPermissions.Pages_FinancialYears, L("FinancialYears"), multiTenancySides: MultiTenancySides.Tenant);
            financialYears.CreateChildPermission(AppPermissions.Pages_FinancialYears_Create, L("CreateNewFinancialYear"), multiTenancySides: MultiTenancySides.Tenant);
            financialYears.CreateChildPermission(AppPermissions.Pages_FinancialYears_Edit, L("EditFinancialYear"), multiTenancySides: MultiTenancySides.Tenant);
            financialYears.CreateChildPermission(AppPermissions.Pages_FinancialYears_Delete, L("DeleteFinancialYear"), multiTenancySides: MultiTenancySides.Tenant);

            var schoolInfos = pages.CreateChildPermission(AppPermissions.Pages_SchoolInfos, L("SchoolInfos"), multiTenancySides: MultiTenancySides.Tenant);
            schoolInfos.CreateChildPermission(AppPermissions.Pages_SchoolInfos_Create, L("CreateNewSchoolInfo"), multiTenancySides: MultiTenancySides.Tenant);
            schoolInfos.CreateChildPermission(AppPermissions.Pages_SchoolInfos_Edit, L("EditSchoolInfo"), multiTenancySides: MultiTenancySides.Tenant);
            schoolInfos.CreateChildPermission(AppPermissions.Pages_SchoolInfos_Delete, L("DeleteSchoolInfo"), multiTenancySides: MultiTenancySides.Tenant);

            var internalGradeSetup = pages.CreateChildPermission(AppPermissions.Pages_InternalGradeSetup, L("InternalGradeSetup"), multiTenancySides: MultiTenancySides.Tenant);
            internalGradeSetup.CreateChildPermission(AppPermissions.Pages_InternalGradeSetup_Create, L("CreateNewInternalGradeSetup"), multiTenancySides: MultiTenancySides.Tenant);
            internalGradeSetup.CreateChildPermission(AppPermissions.Pages_InternalGradeSetup_Edit, L("EditInternalGradeSetup"), multiTenancySides: MultiTenancySides.Tenant);
            internalGradeSetup.CreateChildPermission(AppPermissions.Pages_InternalGradeSetup_Delete, L("DeleteInternalGradeSetup"), multiTenancySides: MultiTenancySides.Tenant);

            var categorySalary = pages.CreateChildPermission(AppPermissions.Pages_CategorySalary, L("CategorySalary"), multiTenancySides: MultiTenancySides.Tenant);
            categorySalary.CreateChildPermission(AppPermissions.Pages_CategorySalary_Create, L("CreateNewCategorySalary"), multiTenancySides: MultiTenancySides.Tenant);
            categorySalary.CreateChildPermission(AppPermissions.Pages_CategorySalary_Edit, L("EditCategorySalary"), multiTenancySides: MultiTenancySides.Tenant);
            categorySalary.CreateChildPermission(AppPermissions.Pages_CategorySalary_Delete, L("DeleteCategorySalary"), multiTenancySides: MultiTenancySides.Tenant);

            var employeeLevels = pages.CreateChildPermission(AppPermissions.Pages_EmployeeLevels, L("EmployeeLevels"), multiTenancySides: MultiTenancySides.Tenant);
            employeeLevels.CreateChildPermission(AppPermissions.Pages_EmployeeLevels_Create, L("CreateNewEmployeeLevel"), multiTenancySides: MultiTenancySides.Tenant);
            employeeLevels.CreateChildPermission(AppPermissions.Pages_EmployeeLevels_Edit, L("EditEmployeeLevel"), multiTenancySides: MultiTenancySides.Tenant);
            employeeLevels.CreateChildPermission(AppPermissions.Pages_EmployeeLevels_Delete, L("DeleteEmployeeLevel"), multiTenancySides: MultiTenancySides.Tenant);

            var schoolLevels = pages.CreateChildPermission(AppPermissions.Pages_SchoolLevels, L("SchoolLevels"), multiTenancySides: MultiTenancySides.Tenant);
            schoolLevels.CreateChildPermission(AppPermissions.Pages_SchoolLevels_Create, L("CreateNewSchoolLevel"), multiTenancySides: MultiTenancySides.Tenant);
            schoolLevels.CreateChildPermission(AppPermissions.Pages_SchoolLevels_Edit, L("EditSchoolLevel"), multiTenancySides: MultiTenancySides.Tenant);
            schoolLevels.CreateChildPermission(AppPermissions.Pages_SchoolLevels_Delete, L("DeleteSchoolLevel"), multiTenancySides: MultiTenancySides.Tenant);


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, PayrollConsts.LocalizationSourceName);
        }
    }
}