using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Chat
{
    /// <summary>
    /// Representa un clienet conectado en línea a la aplicación
    /// </summary>
    public interface IOnlineClient
    {
        /// <summary>
        /// Id único de conexión para este cliente
        /// </summary>
        string ConnectionId
        {
            get;
        }

        /// <summary>
        /// Dirección IP de este cliente
        /// </summary>
        string IpAddress
        {
            get;
        }

        /// <summary>
        /// Id del Tenant
        /// </summary>
        int? TenantId
        {
            get;
        }

        /// <summary>
        /// Id del usuario
        /// </summary>
        long? UserId
        {
            get;
        }

        /// <summary>
        /// Tiempo para establecer conexión para este cliente
        /// </summary>
        DateTime ConnectTime
        {
            get;
        }

        /// <summary>
        /// Atajo para set/get <see cref="P:AlgoriaCore.Application.Chat.IOnlineClient.Properties" />.
        /// </summary>
        object this[string key]
        {
            get;
            set;
        }

        /// <summary>
        /// Puede ser usado para agregar propiedades personalizadas para este cliente
        /// </summary>
        Dictionary<string, object> Properties
        {
            get;
        }
    }
}
