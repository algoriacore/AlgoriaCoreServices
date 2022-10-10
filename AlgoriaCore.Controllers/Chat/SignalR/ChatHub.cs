using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Chat.SignalR
{
    [Authorize]
    //[AlgoriaCoreAuthorizationFilterAttribute]
    public class ChatHub : Hub
    {
        private readonly Stopwatch _timer;
        private readonly IAppSession _session;
        private readonly IChatMessageManager _chatMessageManager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly ICoreLogger _logger;

        public ChatHub(
            IAppSession session, 
            IChatMessageManager chatMessageManager,
            IOnlineClientManager onlineClientManager,
            ICoreLogger logger
            ) : base()
        {
            _timer = new Stopwatch();
            _session = session;
            _chatMessageManager = chatMessageManager;
            _onlineClientManager = onlineClientManager;
            _logger = logger;
        }

        public string SendMessage(SendChatMessageInput input)
        {
            _timer.Start();
            GetSession();

            var sender = new ChatUser(_session.TenantId, _session.UserId.Value);
            var receiver = new ChatUser(input.TenantId, input.UserId);

            try
            {
                _chatMessageManager.SendMessage(sender, receiver, input.Message, input.TenancyName, input.UserName, input.ProfilePictureId);

                return string.Empty;
            }
            catch (Exception ex)
            {
                _timer.Stop();
                input.Message = string.Empty;

                var dict = new Dictionary<string, string>();
                dict.Add("ServiceName", typeof(ChatHub).FullName);
                var method = this.GetType().GetMethod("SendMessage");
                dict.Add("MethodName", method.DeclaringType.FullName + "." + method.Name);
                dict.Add("Parameters", JsonConvert.SerializeObject(input));
                dict.Add("ExecutionDuration", _timer.ElapsedMilliseconds.ToString());
                dict.Add("Exception", ex.ToString());
                dict.Add("Severity", "4");

                _logger.LogError("Could not send chat message to user: " + receiver, dict);
                _logger.LogError(ex, ex.ToString(), dict);

                return "No fue posible enviar el mensaje." + (ex.GetType() == typeof(AlgoriaCoreException) ? " " + ex.Message : "");
            }
        }

        public async override Task OnConnectedAsync()
        {
            GetSession();

            await base.OnConnectedAsync();

            _onlineClientManager.Add(new OnlineClient(Context.ConnectionId, null, _session.TenantId, _session.UserId));
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            GetSession();

            await base.OnDisconnectedAsync(exception);

            _onlineClientManager.Remove(Context.ConnectionId);
        }

        private void GetSession()
        {
            if (/*_session == null &&*/ Context != null)
            {
                _session.TenantId = Context.User.Claims.GetIntValue("TenantId");
                _session.TenancyName = Context.User.Claims.GetStringValue("TenancyName");
                _session.UserId = Context.User.Claims.GetLongValue("UserId");
                _session.UserName = Context.User.Identity.Name;
            }
        }
    }
}
