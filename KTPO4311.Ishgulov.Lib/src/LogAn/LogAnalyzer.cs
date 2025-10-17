using KTPO4311.Ishgulov.Lib.LogAn;

namespace KTPO4311.Ishgulov.Lib.src.LogAn
{
    /// <summary> Анализатор log файлов </summary>

    public class LogAnalyzer
    {
        public LogAnalyzer() { }

        /// <summary>
        /// Проверка правильности имени файла
        /// </summary>
        public bool IsValidLogFileName(string fileName)
        {
            var mgr = ExtensionManagerFactory.Create();
            try
            {
                return mgr.IsValid(fileName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                try
                {
                    IWebService webService = WebServiceFactory.Create();
                    webService.LogError("Слишком короткое имя файла: " + fileName);
                }
                catch (Exception ex)
                {
                    var emailService = EmailServiceFactory.Create();
                    emailService.SendEmail(
                        "admin@company.com",
                        "Ошибка анализа файла",
                        $"Ошибка при анализе {fileName}: {ex.Message}"
                    );
                }
            }
        }
    }

}