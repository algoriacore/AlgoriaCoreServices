using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoriaCore.Application.Authorization
{
    /// <summary>
    /// Usado para guardar y manipular el diccionario de permisos
    /// </summary>
    internal class PermissionDictionary : Dictionary<string, Permission>
    {
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public PermissionDictionary(IAppLocalizationProvider appLocalizationProvider) 
        {
            _appLocalizationProvider = appLocalizationProvider;
        }

        /// <summary>
        /// Agrega todos los permisos hijos de los permisos actuales de forma recursiva
        /// </summary>
        public void AddAllPermissions()
        {
            foreach (var permission in Values.ToList())
            {
                AddPermissionRecursively(permission);
            }
        }

        /// <summary>
        /// Agrega un permisos y todos sus permisos hijos al diccionario
        /// </summary>
        /// <param name="permission">Permiso para ser agregado</param>
        private void AddPermissionRecursively(Permission permission)
        {
            //Previene agregar multiples veces el mismo nombre de permiso
            Permission existingPermission;
            if (TryGetValue(permission.Name, out existingPermission))
            {
                if (existingPermission != permission)
                {
                    throw new EntityDuplicatedException(String.Format(_appLocalizationProvider.L("DuplicatedPermissionName"), permission.Name));
                }
            }
            else
            {
                this[permission.Name] = permission;
            }

            //Agregar permisos hijos (llamada recursiva)
            foreach (var childPermission in permission.Children)
            {
                AddPermissionRecursively(childPermission);
            }
        }
    }
}
