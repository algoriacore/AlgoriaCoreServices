using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using System;

namespace AlgoriaCore.WebUI.Controllers.Model
{
    public class SessionLoginResponseController : SessionLoginResponse
    {
        public String token { get; set; }
    }
}
