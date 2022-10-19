using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;
using Autofac;
using System.Collections.Generic;
using System.Linq;

namespace AlgoriaCore.Application.Authorization
{
    /// <summary>
    /// Proveedor de autorización de la aplicación
    /// Define permisos para la aplicación
    /// Ver <see cref="AppPermissions"/> para todos los nombres de permisos
    /// </summary>
    public class AppAuthorizationProvider : IAppAuthorizationProvider
    {
        private readonly IAppLocalizationProvider _appLocalizationProvider;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILifetimeScope _lifetimeScope;
        private IMongoDBContext _context;

        private Permission _root { get; set; }
        public Permission Root { get
            {
                return _root;
            }
        }

        public AppAuthorizationProvider(
            IAppLocalizationProvider appLocalizationProvider,
            IMultiTenancyConfig multiTenancyConfig,
            ILifetimeScope lifetimeScope,
            IMongoDBContext context
        )
        {
            _appLocalizationProvider = appLocalizationProvider;
            _multiTenancyConfig = multiTenancyConfig;
            _lifetimeScope = lifetimeScope;
            _context = context;

            SetPermissions();
        }

        public Permission GetPermissions()
        {
            SetPermissions();

            return _root;
        }

        public Permission GetPermissions(IAppSession session)
        {
            _root = null;

            SetPermissions(session);

            FilterPermissions(session.TenantId.HasValue ? MultiTenancySides.Tenant : MultiTenancySides.Host);

            return _root;
        }

        private void SetPermissions()
        {
            SetPermissions(null);
        }

        private void SetPermissions(IAppSession session)
        {
            if (_root == null)
            {
                //PERMISOS COMUNES (PARA AMBOS TENANTS AND HOST)

                _root = new Permission(AppPermissions.Pages, L("Pages"));

                var administration = Root.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

                var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
                roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("Create"));
                roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("Edit"));
                roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("Delete"));

                var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
                users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("Create"));
                users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("Edit"));
                users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("Delete"));
                // users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("Users.EditPermissions"));
                users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("Users.Impersonate"));

                var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, "Lenguajes");
                languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("Create"));
                languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("Edit"));
                languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("Delete"));
                languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("Languages.ChangeTexts"));

                var correogrupos = administration.CreateChildPermission(AppPermissions.Pages_Administration_CorreoGrupos, L("EmailGroups"));
                correogrupos.CreateChildPermission(AppPermissions.Pages_Administration_CorreoGrupos_Create, L("Create"));
                correogrupos.CreateChildPermission(AppPermissions.Pages_Administration_CorreoGrupos_Edit, L("Edit"));
                correogrupos.CreateChildPermission(AppPermissions.Pages_Administration_CorreoGrupos_Copy, L("Copy"));
                correogrupos.CreateChildPermission(AppPermissions.Pages_Administration_CorreoGrupos_SetDefault, L("EmailGroups.Select"));

                var tenant_plantillacorreo = administration.CreateChildPermission(AppPermissions.Pages_Administration_PlantillasCorreo, L("EmailTemplates"));
                tenant_plantillacorreo.CreateChildPermission(AppPermissions.Pages_Administration_PlantillasCorreo_Create, L("Create"));
                tenant_plantillacorreo.CreateChildPermission(AppPermissions.Pages_Administration_PlantillasCorreo_Edit, L("Edit"));
                tenant_plantillacorreo.CreateChildPermission(AppPermissions.Pages_Administration_PlantillasCorreo_LoadHTML, L("MailTemplates.LoadHTML"));

                var chatRooms = administration.CreateChildPermission(AppPermissions.Pages_Administration_ChatRooms, L("ChatRooms"));
                chatRooms.CreateChildPermission(AppPermissions.Pages_Administration_ChatRooms_Create, L("Create"));
                chatRooms.CreateChildPermission(AppPermissions.Pages_Administration_ChatRooms_Edit, L("Edit"));

                var helps = administration.CreateChildPermission(AppPermissions.Pages_Administration_Helps, L("Helps"));
                helps.CreateChildPermission(AppPermissions.Pages_Administration_Helps_Create, L("Create"));
                helps.CreateChildPermission(AppPermissions.Pages_Administration_Helps_Edit, L("Edit"));
                helps.CreateChildPermission(AppPermissions.Pages_Administration_Helps_Delete, L("Delete"));

                var orgUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrgUnits, L("OrgUnits"));
                orgUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrgUnits_Create, L("Create"));
                orgUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrgUnits_Edit, L("Edit"));
                orgUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrgUnits_Delete, L("Delete"));

                var mailServiceMailHost = administration.CreateChildPermission(AppPermissions.Pages_Administration_MailServiceMail, L("MailServiceMails.MailServiceMail"));
                mailServiceMailHost.CreateChildPermission(AppPermissions.Pages_Administration_MailServiceMailConfig, L("MailServiceMailConfigs.MailServiceMailConfig"));
                mailServiceMailHost.CreateChildPermission(AppPermissions.Pages_Administration_MailServiceMailAttach, L("MailServiceMailAttachs.MailServiceMailAttach"));

                administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

                var processes = Root.CreateChildPermission(AppPermissions.Pages_Processes, L("Processes"));

                var templates = processes.CreateChildPermission(AppPermissions.Pages_Processes_Templates, L("Templates"));
                templates.CreateChildPermission(AppPermissions.Pages_Processes_Templates_Create, L("Create"));
                templates.CreateChildPermission(AppPermissions.Pages_Processes_Templates_Edit, L("Edit"));
                templates.CreateChildPermission(AppPermissions.Pages_Processes_Templates_Delete, L("Delete"));

                if (!_multiTenancyConfig.IsEnabled())
                {
                    administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"));
                }

                //PERMISOS ESPECÍFICOS TENANT

                administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Tenant.Settings"), multiTenancySides: MultiTenancySides.Tenant);

                Root.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

                var questionnaires = Root.CreateChildPermission(AppPermissions.Pages_Questionnaires, L("Questionnaires"), multiTenancySides: MultiTenancySides.Tenant);

                var questionnaires1 = questionnaires.CreateChildPermission(AppPermissions.Pages_Questionnaires, L("Questionnaires"), multiTenancySides: MultiTenancySides.Tenant);
                questionnaires1.CreateChildPermission(AppPermissions.Pages_Questionnaires_Create, L("Create"), multiTenancySides: MultiTenancySides.Tenant);
                questionnaires1.CreateChildPermission(AppPermissions.Pages_Questionnaires_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Tenant);
                questionnaires1.CreateChildPermission(AppPermissions.Pages_Questionnaires_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

                var catalogsCustom = Root.CreateChildPermission(AppPermissions.Pages_CatalogsCustom, L("CatalogsCustom"), multiTenancySides: MultiTenancySides.Tenant);

                var catalogsCustomConf = catalogsCustom.CreateChildPermission(AppPermissions.Pages_CatalogsCustom, L("CatalogsCustom.Configuration"), multiTenancySides: MultiTenancySides.Tenant);
                catalogsCustomConf.CreateChildPermission(AppPermissions.Pages_CatalogsCustom_Create, L("Create"), multiTenancySides: MultiTenancySides.Tenant);
                catalogsCustomConf.CreateChildPermission(AppPermissions.Pages_CatalogsCustom_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Tenant);
                catalogsCustomConf.CreateChildPermission(AppPermissions.Pages_CatalogsCustom_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

                //PERMISOS ESPECÍFICOS HOST

                administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Host.Settings"), multiTenancySides: MultiTenancySides.Host);

                var tenants = Root.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
                tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("Create"), multiTenancySides: MultiTenancySides.Host);
                tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("Edit"), multiTenancySides: MultiTenancySides.Host);
                tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("Delete"), multiTenancySides: MultiTenancySides.Host);
                tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("Impersonate"), multiTenancySides: MultiTenancySides.Host);

                if (_multiTenancyConfig.IsEnabled())
                {
                    administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: MultiTenancySides.Host);
                }

                if (session != null)
                {
                    MultiTenancySides multiTenancySides = session.TenantId.HasValue ? MultiTenancySides.Tenant : MultiTenancySides.Host;

                    #region TEMPLATES

                    TemplateManager managerTemplate = _lifetimeScope.Resolve<TemplateManager>();

                    using (managerTemplate.CurrentUnitOfWork.SetTenantId(session.TenantId))
                    {
                        List<TemplateDto> templatesListDto = managerTemplate.GetTemplateReadyList();
                        Permission permissionProcess;

                        foreach (TemplateDto templateDto in templatesListDto)
                        {
                            permissionProcess = processes.CreateChildPermission(
                                AppPermissions.CalculatePermissionNameForProcess(AppPermissions.Pages_Processes_Processes, session.TenantId, templateDto.Id.Value),
                                templateDto.NamePlural, multiTenancySides: multiTenancySides);
                            permissionProcess.CreateChildPermission(
                                AppPermissions.CalculatePermissionNameForProcess(AppPermissions.Pages_Processes_Processes_Create, session.TenantId, templateDto.Id.Value),
                                L("Create"), multiTenancySides: multiTenancySides);
                            permissionProcess.CreateChildPermission(
                                AppPermissions.CalculatePermissionNameForProcess(AppPermissions.Pages_Processes_Processes_Edit, session.TenantId, templateDto.Id.Value),
                                L("Edit"), multiTenancySides: multiTenancySides);
                            permissionProcess.CreateChildPermission(
                                AppPermissions.CalculatePermissionNameForProcess(AppPermissions.Pages_Processes_Processes_Delete, session.TenantId, templateDto.Id.Value),
                                L("Delete"), multiTenancySides: multiTenancySides);

                            if (templateDto.IsActivity)
                            {
                                permissionProcess.CreateChildPermission(
                                    AppPermissions.CalculatePermissionNameForProcess(AppPermissions.Pages_Processes_Processes_TimeSheet_Create, session.TenantId, templateDto.Id.Value),
                                    L("Processes.Process.ToDoTimeSheets.Register"), multiTenancySides: multiTenancySides);
                            }
                        }
                    }

                    #endregion

                    #region CATALOGS CUSTOM

                    if (session.TenantId.HasValue && _context.IsEnabled && _context.IsActive)
                    {
                        CatalogCustomManager managerCatalogCustom = _lifetimeScope.Resolve<CatalogCustomManager>();

                        using (managerTemplate.CurrentUnitOfWork.SetTenantId(session.TenantId))
                        {
                            List<CatalogCustomDto> catalogsCustomListDto = managerCatalogCustom.GetCatalogCustomActiveListAsync();
                            Permission permissionCatalogCustom;

                            foreach (CatalogCustomDto catalogCustomDto in catalogsCustomListDto)
                            {
                                permissionCatalogCustom = catalogsCustom.CreateChildPermission(
                                    AppPermissions.CalculatePermissionNameForCatalogCustom(AppPermissions.Pages_CatalogsCustom_CatalogsCustom, session.TenantId, catalogCustomDto.Id),
                                    catalogCustomDto.NamePlural, multiTenancySides: multiTenancySides);
                                permissionCatalogCustom.CreateChildPermission(
                                    AppPermissions.CalculatePermissionNameForCatalogCustom(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Create, session.TenantId, catalogCustomDto.Id),
                                    L("Create"), multiTenancySides: multiTenancySides);
                                permissionCatalogCustom.CreateChildPermission(
                                    AppPermissions.CalculatePermissionNameForCatalogCustom(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Edit, session.TenantId, catalogCustomDto.Id),
                                    L("Edit"), multiTenancySides: multiTenancySides);
                                permissionCatalogCustom.CreateChildPermission(
                                    AppPermissions.CalculatePermissionNameForCatalogCustom(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Delete, session.TenantId, catalogCustomDto.Id),
                                    L("Delete"), multiTenancySides: multiTenancySides);
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        public List<Permission> GetPermissionsFromNamesByValidating(List<string> permissionNames)
        {
            var permissions = new List<Permission>();
            var undefinedPermissionNames = new List<string>();

            foreach (var permissionName in permissionNames)
            {
                var permission = GetPermissionByName(permissionName);
                if (permission == null)
                {
                    undefinedPermissionNames.Add(permissionName);
                }
                else
                {
                    permissions.Add(permission);
                }
            }

            if (undefinedPermissionNames.Count > 0)
            {
                throw new AlgoriaCoreGeneralException(string.Format("Se encontraron {0} permisos no definidos.", undefinedPermissionNames.Count));
            }

            return permissions;
        }

        public Permission GetPermissionByName(string name)
        {
            var ll = GetPermissionList();

            return ll.FirstOrDefault(m => m.Name == name);
        }        

        public List<string> GetPermissionNamesList()
        {
            var treeNode = GetPermissions();

            List<string> resp = new List<string>();
            resp.Add(treeNode.Name);
            resp.AddRange(GetNames(treeNode.Children));

            return resp;
        }

        #region Métodos privados

        public string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }

        private List<string> GetNames(IReadOnlyList<Permission> permissions)
        {
            List<string> resp = new List<string>();
            foreach(var p in permissions)
            {
                resp.Add(p.Name);

                if(p.Children.Count > 0)
                {
                    resp.AddRange(GetNames(p.Children));
                }
            }

            return resp;
        }

        private List<Permission> GetPermissionList()
        {
            var node = _root;

            var list = new List<Permission>();

            GetPermissionList(node, list);

            return list;
        }

        private void GetPermissionList(Permission node, List<Permission> list)
        {
            list.Add(node);

            foreach (var n in node.Children)
            {
                GetPermissionList(n, list);
            }
        }

        private void FilterPermissions(MultiTenancySides side)
        {
            Permission newPermission = null;

            if (_root.MultiTenancySides.HasFlag(side))
            {
                newPermission = new Permission(_root.Name, _root.DisplayName, _root.Description, _root.MultiTenancySides);

                if (_root.Children != null && _root.Children.Count > 0)
                {
                    foreach (var child in _root.Children)
                    {
                        FilterPermissions(side, newPermission, child);
                    }
                }
            }

            _root = newPermission;
        }

        private void FilterPermissions(MultiTenancySides side, Permission newParentPermission, Permission node)
        {
            if (node.MultiTenancySides.HasFlag(side))
            {
                var newPermission = newParentPermission.CreateChildPermission(node.Name, node.DisplayName, node.Description, node.MultiTenancySides);

                if (node.Children != null && node.Children.Count > 0)
                {
                    foreach (var child in node.Children)
                    {
                        FilterPermissions(side, newPermission, child);
                    }
                }
            }
        }

        #endregion
    }
}
