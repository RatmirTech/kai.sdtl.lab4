using KTPO4311.Ishgulov.Lib.LogAn;

/// <summary>
/// Фабрика диспетчеров расширений файлов
/// </summary>
public static class ExtensionManagerFactory
{
    private static IExtensionManager _customManager = null;

    /// <summary>
    /// Создание объекта диспетчера расширений
    /// </summary>
    /// <returns>Объект IExtensionManager</returns>
    public static IExtensionManager Create()
    {
        if (_customManager != null)
        {
            return _customManager;
        }

        return new FileExtensionManager();
    }

    /// <summary>
    /// Метод позволяет тестам контролировать, что возвращает фабрика
    /// </summary>
    /// <param name="mgr">Поддельный диспетчер</param>
    public static void SetManager(IExtensionManager mgr)
    {
        _customManager = mgr;
    }
}