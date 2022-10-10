namespace AlgoriaCore.Domain.Interfaces.Email
{
    public interface IEmailService
    {
        void Send(IEmailMessage message);
    }
}
