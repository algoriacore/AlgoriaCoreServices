using AlgoriaCore.Domain.Interfaces.Email;

namespace AlgoriaInfrastructure.Email
{
    public class TestEmailService : IEmailService
    {
        public TestEmailService()
        {
            
        }

        public void Send(IEmailMessage message)
        {
            // Este servicio no hace nada.
            // Revisar si debe registrar la llamada en algún lugar
        }
    }
}
