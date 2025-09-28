using Clean.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Clean
{
    public class FileService
    {
        public static List<string> GetFileList(string folder)
        {
            var fileList = new List<string>();
            GetFiles(fileList, folder);
            return fileList;
        }

        private static void GetFiles(List<string> fileList, string folder)
        {
            var d = new DirectoryInfo(folder);
            var files = d.GetFiles("*.*");

            foreach (var fileName in files.Select(file => file.FullName))
            {
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext != ".exe" && ext != ".bak" && ext != ".rar" && ext != ".zip")
                {
                    fileList.Add(fileName);
                }
            }
        }

        public static List<Item> GetItemList(List<string> fileList)
        {
            List<Item> items = new List<Item>();
            var count = 0;

            foreach (var file in fileList)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var item = new Item
                {
                    ItemId = count++,
                    Name = fileName,
                    ChangeName = fileName,
                    Changed = false,
                    Path = Path.GetDirectoryName(file),
                    Extension = Path.GetExtension(file)
                };

                items.Add(item);
            }

            return items;
        }

        public static string RemoveEmojis(string filename)
        {
            var emojisToRemove = new[] { "🎃", "🍑", "💦", "🍆", "❌" };

            foreach (var emoji in emojisToRemove)
            {
                filename = filename.Replace(emoji, string.Empty);
            }

            return filename;
        }

        public static string RemoveDiacritics(string filename)
        {
            var normalizedString = filename.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ModifyName(string filename)
        {
            var replacement = " ";
            var charsToRemove = new[] { "_", "`", "´", "“", "”" };


            foreach (var pattern in charsToRemove)
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var number = regex.Matches(filename).Count;

                filename = Regex.Replace(filename, pattern, replacement);
            }

            return filename;
        }

        public static string FixCase(string filename)
        {
            var textInfo = new CultureInfo("en-AU", false).TextInfo;
            filename = textInfo.ToTitleCase(filename);

            return filename;
        }

        public static string CleanFileName(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return filename;

            // Characters to trim from start and end
            char[] trimChars = { '.', '-', '_', ' ', '!' };

            // Trim repeatedly until no more changes occur
            string previous;
            do
            {
                previous = filename;
                filename = filename.Trim(trimChars);
            }
            while (filename != previous);

            return filename;
        }

        public static List<Item> ModifyStatus(List<Item> items)
        {
            var changedItems = items.Where(item => item.Name != item.ChangeName);

            foreach (var item in changedItems)
            {
                item.Changed = true;
            }

            return changedItems.ToList();
        }

        public static void ChangeFileNames(IEnumerable<Item> items)
        {
            foreach (var item in items.Where(item => item.Changed))
            {
                try
                {
                    var oldName = Path.Combine(item.Path ?? string.Empty, item.Name + item.Extension);
                    string newName = string.Empty;
                    if (!string.IsNullOrEmpty(item.Extension))
                    {
                        newName = Path.Combine(item.Path ?? string.Empty, item.ChangeName + item.Extension.ToLowerInvariant());
                    }

                    File.Move(oldName, newName);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex + $"\n{item.ChangeName}");
                }
            }
        }

        public static string RemoveSpaces(string fileName)
        {
            return Regex.Replace(fileName, @"\s+", " ");
        }

        public static void WriteReport(IEnumerable<Item> items)
        {
            var outFile = Environment.CurrentDirectory + "\\alan.log";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var item in items.Where(item => item.Changed))
            {
                var originalName = $"{item.Path}\\{item.Name}{item.Extension}";
                var newName = $"{item.Path}\\{item.ChangeName}{item.Extension}";
                sw.WriteLine($"{originalName}\nto\n{newName}\n\n");
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }
    }
}
