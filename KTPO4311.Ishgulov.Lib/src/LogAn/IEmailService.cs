namespace KTPO4311.Ishgulov.Lib.LogAn
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}