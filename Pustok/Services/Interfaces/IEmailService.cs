namespace Pustok.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(string userEmail, string Subject, string Body, bool IsBody = true);
    }
}
