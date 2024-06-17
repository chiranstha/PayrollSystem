namespace Suktas.Payroll.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        public const string Pages_GradeUpgrades = "Pages.GradeUpgrades";
        public const string Pages_GradeUpgrades_Create = "Pages.GradeUpgrades.Create";
        public const string Pages_GradeUpgrades_Edit = "Pages.GradeUpgrades.Edit";
        public const string Pages_GradeUpgrades_Delete = "Pages.GradeUpgrades.Delete";

        public const string Pages_FestivalBonusSettings = "Pages.FestivalBonusSettings";
        public const string Pages_FestivalBonusSettings_Create = "Pages.FestivalBonusSettings.Create";
        public const string Pages_FestivalBonusSettings_Edit = "Pages.FestivalBonusSettings.Edit";
        public const string Pages_FestivalBonusSettings_Delete = "Pages.FestivalBonusSettings.Delete";

        public const string Pages_PrincipalAllowanceSettings = "Pages.PrincipalAllowanceSettings";
        public const string Pages_PrincipalAllowanceSettings_Create = "Pages.PrincipalAllowanceSettings.Create";
        public const string Pages_PrincipalAllowanceSettings_Edit = "Pages.PrincipalAllowanceSettings.Edit";
        public const string Pages_PrincipalAllowanceSettings_Delete = "Pages.PrincipalAllowanceSettings.Delete";

        public const string Pages_EmployeeSalary = "Pages.EmployeeSalary";
        public const string Pages_EmployeeSalary_Create = "Pages.EmployeeSalary.Create";
        public const string Pages_EmployeeSalary_Edit = "Pages.EmployeeSalary.Edit";
        public const string Pages_EmployeeSalary_Delete = "Pages.EmployeeSalary.Delete";

        public const string Pages_Employees = "Pages.Employees";
        public const string Pages_Employees_Create = "Pages.Employees.Create";
        public const string Pages_Employees_Edit = "Pages.Employees.Edit";
        public const string Pages_Employees_Delete = "Pages.Employees.Delete";

        public const string Pages_FinancialYears = "Pages.FinancialYears";
        public const string Pages_FinancialYears_Create = "Pages.FinancialYears.Create";
        public const string Pages_FinancialYears_Edit = "Pages.FinancialYears.Edit";
        public const string Pages_FinancialYears_Delete = "Pages.FinancialYears.Delete";

        public const string Pages_SchoolInfos = "Pages.SchoolInfos";
        public const string Pages_SchoolInfos_Create = "Pages.SchoolInfos.Create";
        public const string Pages_SchoolInfos_Edit = "Pages.SchoolInfos.Edit";
        public const string Pages_SchoolInfos_Delete = "Pages.SchoolInfos.Delete";

        public const string Pages_InternalGradeSetup = "Pages.InternalGradeSetup";
        public const string Pages_InternalGradeSetup_Create = "Pages.InternalGradeSetup.Create";
        public const string Pages_InternalGradeSetup_Edit = "Pages.InternalGradeSetup.Edit";
        public const string Pages_InternalGradeSetup_Delete = "Pages.InternalGradeSetup.Delete";

        public const string Pages_CategorySalary = "Pages.CategorySalary";
        public const string Pages_CategorySalary_Create = "Pages.CategorySalary.Create";
        public const string Pages_CategorySalary_Edit = "Pages.CategorySalary.Edit";
        public const string Pages_CategorySalary_Delete = "Pages.CategorySalary.Delete";

        public const string Pages_EmployeeLevels = "Pages.EmployeeLevels";
        public const string Pages_EmployeeLevels_Create = "Pages.EmployeeLevels.Create";
        public const string Pages_EmployeeLevels_Edit = "Pages.EmployeeLevels.Edit";
        public const string Pages_EmployeeLevels_Delete = "Pages.EmployeeLevels.Delete";

        public const string Pages_SchoolLevels = "Pages.SchoolLevels";
        public const string Pages_SchoolLevels_Create = "Pages.SchoolLevels.Create";
        public const string Pages_SchoolLevels_Edit = "Pages.SchoolLevels.Edit";
        public const string Pages_SchoolLevels_Delete = "Pages.SchoolLevels.Delete";

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_ChangeProfilePicture = "Pages.Administration.Users.ChangeProfilePicture";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";

        public const string Pages_Administration_MassNotification = "Pages.Administration.MassNotification";
        public const string Pages_Administration_MassNotification_Create = "Pages.Administration.MassNotification.Create";

        public const string Pages_Administration_NewVersion_Create = "Pages_Administration_NewVersion_Create";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";
    }
}