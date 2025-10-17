namespace KTPO4311.Ishgulov.Lib.LogAn;

/// <summary>
/// Интерфейс для проверки расширений файлов
/// </summary>
public interface IExtensionManager
{
    /// <summary>
    /// Проверяет, является ли расширение файла допустимым
    /// </summary>
    /// <param name="fileName">Имя файла</param>
    /// <returns>true, если расширение допустимо</returns>
    bool IsValid(string fileName);
}