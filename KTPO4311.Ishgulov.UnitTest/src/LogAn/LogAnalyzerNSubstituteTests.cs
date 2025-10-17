using KTPO4311.Ishgulov.Lib.LogAn;
using KTPO4311.Ishgulov.Lib.src.LogAn;
using NSubstitute;

namespace KTPO4311.Ishgulov.UnitTest.LogAn;

public class LogAnalyzerNSubstituteTests
{
    [Test]
    public void IsValidFileName_NameSupportedExtension_ReturnsTrue()
    {
        // Подготовка теста с использованием NSubstitute
        var fakeExtensionManager = NSubstitute.Substitute.For<IExtensionManager>();
        fakeExtensionManager.IsValid("test.ext").Returns(true);

        ExtensionManagerFactory.SetManager(fakeExtensionManager);

        LogAnalyzer logAnalyzer = new LogAnalyzer();

        // Воздействие на тестируемый объект
        bool result = logAnalyzer.IsValidLogFileName("test.ext");

        // Проверка ожидаемого результата
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsValidFileName_NoneSupportedExtension_ReturnsFalse()
    {
        // Подготовка теста с использованием NSubstitute
        var fakeExtensionManager = Substitute.For<IExtensionManager>();
        fakeExtensionManager.IsValid("test.bad").Returns(false);

        ExtensionManagerFactory.SetManager(fakeExtensionManager);

        LogAnalyzer logAnalyzer = new LogAnalyzer();

        // Воздействие на тестируемый объект
        bool result = logAnalyzer.IsValidLogFileName("test.bad");

        // Проверка ожидаемого результата
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
    {
        // Подготовка теста с использованием NSubstitute
        var fakeExtensionManager = Substitute.For<IExtensionManager>();
        fakeExtensionManager.When(x => x.IsValid("test.txt"))
                            .Do(x => { throw new Exception("Test exception"); });

        ExtensionManagerFactory.SetManager(fakeExtensionManager);

        LogAnalyzer logAnalyzer = new LogAnalyzer();

        // Воздействие на тестируемый объект
        bool result = logAnalyzer.IsValidLogFileName("test.txt");

        // Проверка ожидаемого результата
        Assert.That(result, Is.False);
    }

    [Test]
    public void Analyze_TooShortFileName_CallsWebService()
    {
        // Подготовка поддельного веб-сервиса
        var mockWebService = Substitute.For<IWebService>();
        WebServiceFactory.SetService(mockWebService);

        var log = new LogAnalyzer();
        string tooShortFileName = "abc.ext";

        // Воздействие на тестируемый объект
        log.Analyze(tooShortFileName);

        // Проверка, что веб-сервис получил ожидаемый вызов
        mockWebService.Received().LogError("Слишком короткое имя файла: abc.ext");
    }

    [Test]
    public void Analyze_WebServiceThrows_SendsEmail()
    {
        // Подготовка поддельного веб-сервиса, который вызывает исключение
        IWebService stubWebService = Substitute.For<IWebService>();
        stubWebService.When(x => x.LogError(Arg.Any<string>()))
                    .Do(x => { throw new Exception("Web service exception"); });
        WebServiceFactory.SetService(stubWebService);

        // Подготовка поддельного почтового сервиса
        IEmailService mockEmail = Substitute.For<IEmailService>();
        EmailServiceFactory.SetService(mockEmail);

        var log = new LogAnalyzer();
        string tooShortFileName = "abc.ext";

        // Воздействие на тестируемый объект
        log.Analyze(tooShortFileName);

        mockEmail.Received().SendEmail(
            "admin@company.com",
            "Ошибка анализа файла",
            $"Ошибка при анализе {tooShortFileName}: Web service exception"
        );
    }

    [TearDown]
    public void TearDown()
    {
        ExtensionManagerFactory.SetManager(null);
        WebServiceFactory.SetService(null);
        EmailServiceFactory.SetService(null);
    }
}