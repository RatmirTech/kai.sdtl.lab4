namespace KTPO4311.Ishgulov.Lib.LogAn
{
    public static class WebServiceFactory
    {
        private static IWebService _webService;

        public static void SetService(IWebService service)
        {
            _webService = service;
        }

        public static IWebService Create()
        {
            return _webService ?? new WebService();
        }
    }
}