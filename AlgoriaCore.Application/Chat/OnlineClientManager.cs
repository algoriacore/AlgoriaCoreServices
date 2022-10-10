using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AlgoriaCore.Application.Chat
{
    public class OnlineClientManager : IOnlineClientManager
    {
        protected readonly object SyncObj = new object();

        /// <summary>
        /// Clientes en línea
        /// </summary>
        protected ConcurrentDictionary<string, IOnlineClient> Clients
        {
            get;
        }

        public event EventHandler<OnlineClientEventArgs> ClientConnected;

        public event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        public event EventHandler<OnlineUserEventArgs> UserConnected;

        public event EventHandler<OnlineUserEventArgs> UserDisconnected;

        public OnlineClientManager()
        {
            Clients = new ConcurrentDictionary<string, IOnlineClient>();
        }

        public virtual void Add(IOnlineClient client)
        {
            lock (SyncObj)
            {
                bool flag = false;
                ChatUser userIdentifier = client.ToUserIdentifierOrNull();

                if (userIdentifier != null)
                {
                    flag = this.IsOnline(userIdentifier);
                }

                Clients[client.ConnectionId] = client;
                ClientConnected.InvokeSafely(this, new OnlineClientEventArgs(client));

                if (userIdentifier != null && !flag)
                {
                    System.Diagnostics.Debug.WriteLine("Se agrega el primer cliente para el Tenant: " + userIdentifier.TenantId + " Usuario: " + userIdentifier.UserId);
                    UserConnected.InvokeSafely(this, new OnlineUserEventArgs(userIdentifier, client));
                }
            }
        }

        public virtual bool Remove(string connectionId)
        {
            lock (SyncObj)
            {
                IOnlineClient onlineClient = default(IOnlineClient);
                bool num = Clients.TryRemove(connectionId, out onlineClient);

                if (num)
                {
                    ChatUser userIdentifier = onlineClient.ToUserIdentifierOrNull();

                    if (userIdentifier != null && !this.IsOnline(userIdentifier))
                    {
                        System.Diagnostics.Debug.WriteLine("Ya no se encontraron clientes para el Tenant: " + userIdentifier.TenantId + " Usuario: " + userIdentifier.UserId);
                        UserDisconnected.InvokeSafely(this, new OnlineUserEventArgs(userIdentifier, onlineClient));
                    }
                    
                    ClientDisconnected.InvokeSafely(this, new OnlineClientEventArgs(onlineClient));
                }

                return num;
            }
        }

        public virtual IOnlineClient GetByConnectionIdOrNull(string connectionId)
        {
            lock (SyncObj)
            {
                return Clients.GetOrDefault(connectionId);
            }
        }

        public virtual IReadOnlyList<IOnlineClient> GetAllClients()
        {
            //IL_001c: Tipo de resultado desconocido (puede ser causado por un IL inválido o referencias faltantes)
            //IL_0021: Esperado O, pero obtenido Desconocido
            lock (SyncObj)
            {
                return (IReadOnlyList<IOnlineClient>)ImmutableList.ToImmutableList<IOnlineClient>((IEnumerable<IOnlineClient>)this.Clients.Values);
            }
        }

        public virtual IReadOnlyList<IOnlineClient> GetAllByUserId(IChatUser user)
        {
            //IL_0035: Tipo de resultado desconocido (puede ser causado por un IL inválido o referencias faltantes)
            //IL_003a: Esperado O, pero obtenido Desconocido
            Check.NotNull(user, "user");
            return (IReadOnlyList<IOnlineClient>)ImmutableList.ToImmutableList<IOnlineClient>(this.GetAllClients().Where(delegate (IOnlineClient c)
            {
                if (c.UserId == user.UserId)
                {
                    return c.TenantId == user.TenantId;
                }
                return false;
            }));
        }





        



    }

    public static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(string.Format("{0} can not be null, empty or white space!", parameterName), parameterName);
            }
            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }
            return value;
        }

        public static ChatUser ToUserIdentifierOrNull(this IOnlineClient onlineClient)
        {
            if (!onlineClient.UserId.HasValue)
            {
                return null;
            }
            return new ChatUser(onlineClient.TenantId, onlineClient.UserId.Value);
        }

        /// <summary>
        /// Determina si el usuario especificado está en linea o no
        /// </summary>
        /// <param name="onlineClientManager">Ela dministrador de clientes en línea.</param>
        /// <param name="user">Usuario.</param>
        public static bool IsOnline(this IOnlineClientManager onlineClientManager, ChatUser user)
        {
            return onlineClientManager.GetAllByUserId(user).Any();
        }

        /// <summary>
        /// Lanza un evento dato de forma segura con los argumentos proporcionados
        /// </summary>
        /// <typeparam name="TEventArgs">Tipo <see cref="T:System.EventArgs" /></typeparam>
        /// <param name="eventHandler">El manejador del evento</param>
        /// <param name="sender">Fuente del evento</param>
        /// <param name="e">Argumento del evento</param>
        public static void InvokeSafely<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }
    }



}
