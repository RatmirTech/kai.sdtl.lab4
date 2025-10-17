using System;
using System.Configuration;
using System.Linq;

namespace KTPO4311.Ishgulov.Lib.LogAn
{
    /// <summary>
    /// Реализация менеджера расширений, читающая допустимые расширения из app.config
    /// </summary>
    public class FileExtensionManager : IExtensionManager
    {
        private readonly string[] _allowedExtensions;

        public FileExtensionManager()
        {
            var extensionsString = ConfigurationManager.AppSettings["AllowedExtensions"];
            if (string.IsNullOrWhiteSpace(extensionsString))
            {
                _allowedExtensions = Array.Empty<string>();
            }
            else
            {
                _allowedExtensions = extensionsString
                    .Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ext => ext.Trim().ToLowerInvariant())
                    .ToArray();
            }
        }

        /// <summary>
        /// Проверяет, поддерживается ли расширение файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>True, если расширение в списке разрешённых</returns>
        public bool IsValid(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var extension = System.IO.Path.GetExtension(fileName)?.TrimStart('.').ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }
    }
}