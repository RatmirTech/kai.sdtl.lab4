using System;
using KTPO4311.Ishgulov.Lib.LogAn;
using KTPO4311.Ishgulov.Lib.src.LogAn;

namespace KTPO4311.Ishgulov.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Клиентское приложение KTPO4311.Ishgulov.Service ===\n");
            Console.WriteLine("Проверка файлов на допустимость расширений...\n");

            var analyzer = new LogAnalyzer();

            // Тестовые файлы
            string[] testFiles = {
                "document.ishgulov",
                "data.log",
                "report.txt",
                "backup.dat",
                "image.jpg",
                "script.py",
                "config.ini",
                "file.",           // пустое расширение
                "noextension"      // без точки
            };

            foreach (var file in testFiles)
            {
                bool isValid = analyzer.IsValidLogFileName(file);
                string status = isValid ? "✅ ДОПУСТИМ" : "❌ НЕ ДОПУСТИМ";
                Console.WriteLine($"{file,-20} → {status}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}