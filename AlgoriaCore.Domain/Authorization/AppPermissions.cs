using System;
using System.Linq;

namespace AlgoriaCore.Domain.Authorization
{
    /// <summary>
    /// Define cadenas constantes para los nombres de los permisos de la aplicación
    /// <see cref="AppAuthorizationProvider"/> para la definición de permisos.
    /// </summary>
    public static class AppPermissions
    {
        //PERMISOS COMUNES (PARA AMBOS TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_CorreoGrupos = "conftcor.0";
        public const string Pages_Administration_CorreoGrupos_Create = "conftcor.1";
        public const string Pages_Administration_CorreoGrupos_Edit = "conftcor.2";
        public const string Pages_Administration_CorreoGrupos_Copy = "conftcor.3";
        public const string Pages_Administration_CorreoGrupos_SetDefault = "conftcor.7";

        public const string Pages_Administration_PlantillasCorreo = "conftcor.4";
        public const string Pages_Administration_PlantillasCorreo_Create = "conftcor.5";
        public const string Pages_Administration_PlantillasCorreo_Edit = "conftcor.6";
        public const string Pages_Administration_PlantillasCorreo_LoadHTML = "conftcor.8";

        public const string Pages_Administration_ChatRooms = "Pages.Administration.ChatRooms";
        public const string Pages_Administration_ChatRooms_Create = "Pages.Administration.ChatRooms.Create";
        public const string Pages_Administration_ChatRooms_Edit = "Pages.Administration.ChatRooms.Edit";

        public const string Pages_Administration_Helps = "Pages.Administration.Helps";
        public const string Pages_Administration_Helps_Create = "Pages.Administration.Helps.Create";
        public const string Pages_Administration_Helps_Edit = "Pages.Administration.Helps.Edit";
        public const string Pages_Administration_Helps_Delete = "Pages.Administration.Helps.Delete";

        public const string Pages_Administration_OrgUnits = "Pages.Administration.OrgUnits";
        public const string Pages_Administration_OrgUnits_Create = "Pages.Administration.OrgUnits.Create";
        public const string Pages_Administration_OrgUnits_Edit = "Pages.Administration.OrgUnits.Edit";
        public const string Pages_Administration_OrgUnits_Delete = "Pages.Administration.OrgUnits.Delete";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        // PERMISOS DE CORREO ELECTRÓNICO
        public const string Pages_Administration_MailServiceMail = "Pages.Administration.MailServiceMail";
        public const string Pages_Administration_MailServiceMailAttach = "Pages.Administration.MailServiceMailAttach";
        public const string Pages_Administration_MailServiceMailConfig = "Pages.Administration.MailServiceMailConfig";



        public const string Pages_Processes = "Pages.Processes";

        public const string Pages_Processes_Templates = "Pages.Processes.Templates";
        public const string Pages_Processes_Templates_Create = "Pages.Processes.Templates.Create";
        public const string Pages_Processes_Templates_Edit = "Pages.Processes.Templates.Edit";
        public const string Pages_Processes_Templates_Delete = "Pages.Processes.Templates.Delete";

        public const string Pages_Processes_Processes = "Pages.Processes.Processes_{processId}";
        public const string Pages_Processes_Processes_Create = "Pages.Processes.Processes_{processId}.Create";
        public const string Pages_Processes_Processes_Edit = "Pages.Processes.Processes_{processId}.Edit";
        public const string Pages_Processes_Processes_Delete = "Pages.Processes.Processes_{processId}.Delete";

        public const string Pages_Processes_Processes_TimeSheet_Create = "Pages.Processes.Processes_{processId}.TimeSheet.Create";

        //PERMISOS ESPECÍFICOS DEL TENANT

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Tenant_Catalogos = "Pages.Tenant.Catalogos";

        public const string Pages_Questionnaires = "Pages.Questionnaires";
        public const string Pages_Questionnaires_Create = "Pages.Questionnaires.Create";
        public const string Pages_Questionnaires_Edit = "Pages.Questionnaires.Edit";
        public const string Pages_Questionnaires_Delete = "Pages.Questionnaires.Delete";

        public const string Pages_CatalogsCustom = "Pages.CatalogsCustom";
        public const string Pages_CatalogsCustom_Create = "Pages.CatalogsCustom.Create";
        public const string Pages_CatalogsCustom_Edit = "Pages.CatalogsCustom.Edit";
        public const string Pages_CatalogsCustom_Delete = "Pages.CatalogsCustom.Delete";

        // PERMISOS DE CORRO ELECTRÓNICO
        public const string Pages_Tenants_MailServiceMail = "Pages.Tenants.MailServiceMail";
        public const string Pages_Tenants_MailServiceMailAttach = "Pages.Tenants.MailServiceMailAttach";
        public const string Pages_Tenants_MailServiceMailConfig = "Pages.Tenants.MailServiceMailConfig";

        public const string Pages_CatalogsCustom_CatalogsCustom = "Pages.CatalogsCustom.CatalogsCustom_{catalogId}";
        public const string Pages_CatalogsCustom_CatalogsCustom_Create = "Pages.CatalogsCustom.CatalogsCustom_{catalogId}.Create";
        public const string Pages_CatalogsCustom_CatalogsCustom_Edit = "Pages.CatalogsCustom.CatalogsCustom_{catalogId}.Edit";
        public const string Pages_CatalogsCustom_CatalogsCustom_Delete = "Pages.CatalogsCustom.CatalogsCustom_{catalogId}.Delete";

        //PERMISOS ESPECÍFICOS DEL HOST

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";

        #region Process

        public static string CalculatePermissionNameForProcess(string permissionName, int? tenantId, long templateId) {
            string processId = (tenantId == null ? "" : tenantId.Value.ToString() + "_") + templateId.ToString();

            return permissionName.Replace("{processId}", processId);
        }

        public static bool IsPermissionNameForProcess(string permissionName)
        {
            string[] permissionNames = new string[] { permissionName };

            return HasPermissionNameForProcess(permissionNames);
        }

        public static bool HasPermissionNameForProcess(string[] permissionNames)
        {
            return permissionNames.Contains(Pages_Processes_Processes)
            || permissionNames.Contains(Pages_Processes_Processes_Create)
            || permissionNames.Contains(Pages_Processes_Processes_Edit)
            || permissionNames.Contains(Pages_Processes_Processes_Delete)
            || permissionNames.Contains(Pages_Processes_Processes_TimeSheet_Create);
        }

        #endregion

        #region Catalogs Custom

        public static string CalculatePermissionNameForCatalogCustom(string permissionName, int? tenantId, string catalogId)
        {
            string processId = (tenantId == null ? "" : tenantId.Value.ToString() + "_") + catalogId;

            return permissionName.Replace("{catalogId}", processId);
        }

        public static bool IsPermissionNameForCatalogCustom(string permissionName)
        {
            string[] permissionNames = new string[] { permissionName };

            return HasPermissionNameForCatalogCustom(permissionNames);
        }

        public static bool HasPermissionNameForCatalogCustom(string[] permissionNames)
        {
            return permissionNames.Contains(Pages_CatalogsCustom_CatalogsCustom)
            || permissionNames.Contains(Pages_CatalogsCustom_CatalogsCustom_Create)
            || permissionNames.Contains(Pages_CatalogsCustom_CatalogsCustom_Edit)
            || permissionNames.Contains(Pages_CatalogsCustom_CatalogsCustom_Delete);
        }

        #endregion
    }
}
