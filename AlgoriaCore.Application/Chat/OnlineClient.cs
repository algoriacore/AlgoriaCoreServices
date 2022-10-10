using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Chat
{
    /// <summary>
	/// Implementa <see cref="T:AlgoriaCore.Application.Chat.IOnlineClient" />.
	/// </summary>
	[Serializable]
    public class OnlineClient : IOnlineClient
    {
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Id único de conexión para este cliente
        /// </summary>
        public string ConnectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Dirección IP de este cliente
        /// </summary>
        public string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Id de Tenant
        /// </summary>
        public int? TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Id de usuario
        /// </summary>
        public long? UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Tiempo para establecer conexión para este cliente
        /// </summary>
        public DateTime ConnectTime
        {
            get;
            set;
        }

        /// <summary>
        /// Atajo para set/get <see cref="P:AlgoriaCore.Application.Chat.OnlineClient.Properties" />.
        /// </summary>
        public object this[string key]
        {
            get
            {
                return this.Properties[key];
            }
            set
            {
                this.Properties[key] = value;
            }
        }

        /// <summary>
        /// Puede ser usado para agregar propiedades personalizadas para este cliente
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _properties = value;
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="T:AlgoriaCore.Application.Chat.OnlineClient" />.
        /// </summary>
        public OnlineClient()
        {
            this.ConnectTime = DateTime.Now;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="T:AlgoriaCore.Application.Chat.OnlineClient" />.
        /// </summary>
        /// <param name="connectionId">el identificador de conexión.</param>
        /// <param name="ipAddress">La dirección ip.</param>
        /// <param name="tenantId">El identificador del tenant.</param>
        /// <param name="userId">El usuario identificador.</param>
        public OnlineClient(string connectionId, string ipAddress, int? tenantId, long? userId) : this()
        {
            this.ConnectionId = connectionId;
            this.IpAddress = ipAddress;
            this.TenantId = tenantId;
            this.UserId = userId;
            this.Properties = new Dictionary<string, object>();
        }
    }
}
