using NUnit.Framework;
using KTPO4311.Ishgulov.Lib.LogAn;
using KTPO4311.Ishgulov.Lib.src.LogAn;
using NUnit.Framework.Legacy;

namespace KTPO4311.Ishgulov.UnitTest.LogAn
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        public void IsValidFileName_NameSupportedExtension_ReturnsTrue()
        {
            var fakeMgr = new FakeExtensionManager { WillReturn = true };
            ExtensionManagerFactory.SetManager(fakeMgr);

            var analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName("file.ishgulov");
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidFileName_NameUnsupportedExtension_ReturnsFalse()
        {
            var fakeMgr = new FakeExtensionManager { WillReturn = false };
            ExtensionManagerFactory.SetManager(fakeMgr);

            var analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName("file.txt");
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
        {
            var fakeMgr = new FakeExtensionManager 
            { 
                WillThrow = new Exception("Ошибка при проверке расширения") 
            };
            ExtensionManagerFactory.SetManager(fakeMgr);
            
            var analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName("file.ishgulov");
            Assert.That(result, Is.False);
        }

        [TearDown]
        public void AfterEachTest()
        {
            ExtensionManagerFactory.SetManager(null);   
            WebServiceFactory.SetService(null);
            EmailServiceFactory.SetService(null);
        }

        [Test]
        public void Analyze_TooShortFileName_CallsWebService()
        {
            var mockWebService = new FakeWebService();
            WebServiceFactory.SetService(mockWebService);

            var analyzer = new LogAnalyzer();

            analyzer.Analyze("a.txt");

            Assert.That(mockWebService.LastError, Is.EqualTo("Слишком короткое имя файла: a.txt"));
        }
        
        [Test]
        public void Analyze_WebServiceThrows_SendsEmail()
        {
            var stubWebService = new FakeWebService();
            stubWebService.WillThrow = new Exception("Ошибка веб-службы");

            var mockEmailService = new FakeEmailService();
            EmailServiceFactory.SetService(mockEmailService);
            WebServiceFactory.SetService(stubWebService);

            var analyzer = new LogAnalyzer();

            analyzer.Analyze("a.txt");

            Assert.That(mockEmailService.WasCalled, Is.True);
            Assert.That(mockEmailService.To, Is.EqualTo("admin@company.com"));
            Assert.That(mockEmailService.Subject, Is.EqualTo("Ошибка анализа файла"));
            Assert.That(mockEmailService.Body, Contains.Substring("Ошибка при анализе a.txt"));
        }
    }

    /// <summary>
    /// Поддельный менеджер расширений
    /// </summary>
    public class FakeExtensionManager : IExtensionManager
    {
        public bool WillReturn { get; set; } = true;
        public Exception WillThrow { get; set; }

        public bool IsValid(string fileName)
        {
            if (WillThrow != null)
                throw WillThrow;

            return WillReturn;
        }
    }
    
    public class FakeWebService : IWebService
    {
        public string LastError;
        
        public Exception WillThrow = null;

        public void LogError(string message)
        {
            if (WillThrow != null)
            {
                throw WillThrow;
            }
            LastError = message;
        }
    }
    
    public class FakeEmailService : IEmailService
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool WasCalled { get; private set; }

        public void SendEmail(string to, string subject, string body)
        {
            WasCalled = true;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}