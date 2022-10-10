using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Chat
{
    /// <summary>
    /// Usado para administrar los clientes en línea que están conectados a la aplicación
	/// </summary>
	public interface IOnlineClientManager
    {
        event EventHandler<OnlineClientEventArgs> ClientConnected;

        event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        event EventHandler<OnlineUserEventArgs> UserConnected;

        event EventHandler<OnlineUserEventArgs> UserDisconnected;

        /// <summary>
        /// Agregar un cliente
        /// </summary>
        /// <param name="client">El cliente.</param>
        void Add(IOnlineClient client);

        /// <summary>
        /// Elimina un cliente por id de conexión
        /// </summary>
        /// <param name="connectionId">El id de conexión.</param>
        /// <returns>Verdadero, si el cliente es removido</returns>
        bool Remove(string connectionId);

        /// <summary>
        /// Intenta encontrar un cliente por id de conexión
        /// Regresa nulo sino lo encuentra
        /// </summary>
        /// <param name="connectionId">id de conexión</param>
        IOnlineClient GetByConnectionIdOrNull(string connectionId);

        /// <summary>
        /// Obtiene todos los cliens en línea
        /// </summary>
        IReadOnlyList<IOnlineClient> GetAllClients();

        IReadOnlyList<IOnlineClient> GetAllByUserId(IChatUser user);
    }
}
