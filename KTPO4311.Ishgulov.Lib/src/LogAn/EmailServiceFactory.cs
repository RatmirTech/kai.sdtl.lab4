namespace KTPO4311.Ishgulov.Lib.LogAn
{
    public static class EmailServiceFactory
    {
        private static IEmailService _emailService;

        public static void SetService(IEmailService service)
        {
            _emailService = service;
        }

        public static IEmailService Create()
        {
            return _emailService ?? new EmailService();
        }
    }
}