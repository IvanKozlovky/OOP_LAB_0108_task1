using System;
using System.IO;

namespace FileSystemOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            // Замініть наступні значення на ваші власні
            string groupNumber = "GroupNumber";
            string lastName = "LastName";

            string basePath = @"D:\OOP_lab08";
            string groupPath = Path.Combine(basePath, groupNumber);
            string lastNamePath = Path.Combine(basePath, lastName);
            string sourcesPath = Path.Combine(basePath, "Sources");
            string reportsPath = Path.Combine(basePath, "Reports");
            string textsPath = Path.Combine(basePath, "Texts");

            try
            {
                // Створення каталогів
                Directory.CreateDirectory(basePath);
                Directory.CreateDirectory(groupPath);
                Directory.CreateDirectory(lastNamePath);
                Directory.CreateDirectory(sourcesPath);
                Directory.CreateDirectory(reportsPath);
                Directory.CreateDirectory(textsPath);

                // Копіювання каталогів
                DirectoryCopy(textsPath, Path.Combine(lastNamePath, "Texts"));
                DirectoryCopy(sourcesPath, Path.Combine(lastNamePath, "Sources"));
                DirectoryCopy(reportsPath, Path.Combine(lastNamePath, "Reports"));

                // Переміщення каталогу
                Directory.Move(lastNamePath, Path.Combine(groupPath, lastName));

                // Створення текстового файлу
                string dirInfoPath = Path.Combine(groupPath, lastName, "Texts", "dirinfo.txt");
                using (StreamWriter sw = File.CreateText(dirInfoPath))
                {
                    DirectoryInfo di = new DirectoryInfo(Path.Combine(groupPath, lastName));
                    sw.WriteLine($"DirectoryName: {di.Name}");
                    sw.WriteLine($"Parent Directory: {di.Parent.Name}");
                    sw.WriteLine($"Full Path: {di.FullName}");
                    sw.WriteLine($"Creation Time: {di.CreationTime}");
                }

                Console.WriteLine("Файлова структура успішно створена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Каталог-джерело не існує або не може бути знайдений: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }
    }
}
