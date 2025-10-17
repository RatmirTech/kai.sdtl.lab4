using KTPO4311.Ishgulov.Lib.LogAn;
using NSubstitute;
namespace KTPO4311.Ishgulov.UnitTest.Sample;

public class SampleNSubstituteTests
{
    [Test]
    public void Returns_ParticularArg_Works()
    {
        IExtensionManager fakeExtensionManager = Substitute.For<IExtensionManager>();

        // Настроить поддельный объект: для конкретного аргумента вернуть true
        fakeExtensionManager.IsValid("validfile.ext").Returns(true);

        // Вызов тестируемого метода
        bool result = fakeExtensionManager.IsValid("validfile.ext");

        // Проверка ожидаемого результата
        Assert.That(result, Is.True);
    }

    [Test]
    public void Returns_ArgAny_Works()
    {
        IExtensionManager fakeExtensionManager = Substitute.For<IExtensionManager>();
        // Настроить поддельный объект: для любого аргумента вернуть true
        fakeExtensionManager.IsValid(Arg.Any<string>()).Returns(true);

        // Воздействие на тестируемый метод с любым аргументом
        bool result = fakeExtensionManager.IsValid("file1.ext");

        // Проверка ожидаемого результата
        Assert.That(result, Is.True);
    }

    [Test]
    public void Returns_ArgAny_Throws()
    {
        IExtensionManager fakeExtensionManager = Substitute.For<IExtensionManager>();

        // Настроить поддельный объект: для любого аргумента выбросить исключение
        fakeExtensionManager.When(x => x.IsValid(Arg.Any<string>()))
                            .Do(_ => { throw new Exception("fake exception"); });

        // Проверка, что вызов метода с любым аргументом выбрасывает исключение
        Assert.Throws<Exception>(() => fakeExtensionManager.IsValid("anything"));
    }
    
    [Test]
    public void Received_ParticularArg_Saves()
    {
        // Создать поддельный объект
        IWebService mockWebService = Substitute.For<IWebService>();

        // Воздействие на поддельный объект
        mockWebService.LogError("Поддельное сообщение");

        // Проверка, что поддельный объект получил вызов
        mockWebService.Received().LogError("Поддельное сообщение");
    }
}