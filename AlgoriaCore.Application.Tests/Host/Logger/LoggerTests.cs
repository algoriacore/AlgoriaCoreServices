using AlgoriaCore.Application.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Host.Logger
{
    [Collection("TestsCollection")]
    public class LoggerTests : TestBaseHost
    {
        public LoggerTests(QueryTestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task LogCriticalTest()
        {
            var args = new Dictionary<string, string>();
            args.Add("EmailSubject", "Mensaje desde pruebas.");
            args.Add("EmailTo", "pzavala@algoria.com.mx");
            args.Add("EmailFrom", "no-replay@algoria.com.mx");
            args.Add("EmailSmtpServer", "mail.algoria.com.mx");
            args.Add("EmailSmtpPort", "9025");
            args.Add("EmailSmtpUserName", "no-replay@algoria.com.mx");
            args.Add("EmailSmtpPassword", "34670125");
            args.Add("EmailSmtpAuthentication", "Basic");
            args.Add("EmailSkipCertificateValidation", "true");
            args.Add("EmailHtml", "false");
            args.Add("EmailBody", "Este es el cuerpo de un error de log crítico.");

            args.Add("Severity", "5"); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5

            bool resp = await Task.Run(() =>
            {
                Logger.LogCritical("Mensaje de prueba crítico.", args);
                return true;
            });

            Assert.True(resp);
        }
    }
}
