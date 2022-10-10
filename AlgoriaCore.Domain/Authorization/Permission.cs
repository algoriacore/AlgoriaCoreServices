using AlgoriaCore.Domain.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AlgoriaCore.Domain.Authorization
{
    /// <summary>
    /// Repressenta un permiso.
    /// Un permiso es usado para restringir funcionalidades de la aplicación de usuarios no autorizados
    /// </summary>	
    public sealed class Permission
    {
        /// <summary>
        /// Padre de este permisos si existe.
        /// Si se establece, este permiso puede ser concedido solo si el padre es concedido.
        /// </summary>
        public Permission Parent { get; private set; }

        /// <summary>
        /// Nombre único del permiso
        /// Este es el nombre clave para conceder permisos
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Nombre para desplegar del permiso
        /// Este puede ser usado para mostrar el permiso al usuario
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Una breve descripción para este permiso.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Cuál lado puede usar este permiso.
        /// </summary>
        public MultiTenancySides MultiTenancySides { get; set; }

        /// <summary>
        /// Lista de permisos hijos. Un pemriso hijo puede ser concedido solo si el padre es concedido.
        /// </summary>
        public IReadOnlyList<Permission> Children => _children.ToImmutableList();
        private readonly List<Permission> _children;

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        /// <param name="name">Nombre único del permiso</param>
        /// <param name="displayName">Nombre para desplegar del permiso</param>
        /// <param name="description">Una breve descripción para este permiso</param>
        /// <param name="multiTenancySides">Cuál lado puede usar este permiso</param>
        /// <param name="featureDependency">Características dependientes de este permiso</param>
        public Permission(
            string name,
            //ILocalizableString displayName = null,
            //ILocalizableString description = null,
            string displayName = null,
            string description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant)//,
            //IFeatureDependency featureDependency = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            DisplayName = displayName;
            Description = description;
            MultiTenancySides = multiTenancySides;

            _children = new List<Permission>();
        }

        /// <summary>
        /// Agrega un permiso hijo.
        /// Un permiso hijo solo puede ser concedido si el padre es concedido.
        /// </summary>
        /// <returns>Returns newly created child permission</returns>
        public Permission CreateChildPermission(
            string name,
            //ILocalizableString displayName = null,
            //ILocalizableString description = null,
            string displayName = null,
            string description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant)//,
            //IFeatureDependency featureDependency = null)
        {
            var permission = new Permission(name, displayName, description, multiTenancySides) { Parent = this };
            _children.Add(permission);
            return permission;
        }

        public override string ToString()
        {
            return string.Format("[Permission: {0}]", Name);
        }
    }
}
