using AlgoriaCore.Domain.MultiTenancy;

namespace AlgoriaCore.Domain.Authorization
{
    /// <summary>
    /// Defines standard interface for authorization attributes.
    /// Define una interfaz estándar para atributos de autorización
    /// </summary>
    public interface IAlgoriaCoreAuthorizeAttribute
    {
        /// <summary>
        /// Una lista de permisos para autorizar.
        /// </summary>
        string[] Permissions { get; }

        /// <summary>
        /// Si esta propiedad se establece en verdadero, todos los <see cref="Permissions"/> deben ser concedidos.
        /// Si es falso, al menos uno de los <see cref="Permissions"/> debe ser concedido.
        /// Predeterminado: false.
        /// </summary>
        bool RequireAllPermissions { get; set; }

        byte MultiTenancySide { get; set; }
    }
}
