using Clean.Models;
using System.Diagnostics;

namespace Clean
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var argList = ArgumentService.GetArguments(args.ToList());
                       
            var fileDirectory = Environment.CurrentDirectory;

            List<string> fileList = FileService.GetFileList(fileDirectory, argList.SubFolder);

            List<Item> items = new List<Item>();
            items = FileService.GetItemList(fileList);

            // Process the filenames
            foreach (var item in items)
            {
                item.ChangeName = FileService.ChangeCharacter(item.ChangeName);
                item.ChangeName = FileService.RemoveEmojis(item.ChangeName);
                item.ChangeName = FileService.RemoveDiacritics(item.ChangeName);
                item.ChangeName = FileService.ModifyName(item.ChangeName);

                if (item.ChangeName == item.ChangeName.ToUpperInvariant() || argList.ProperCase == true)
                {
                    item.ChangeName = item.ChangeName.ToLowerInvariant(); // .ToTitleCase() won't change uppercase filenames
                    item.ChangeName = FileService.FixCase(item.ChangeName);
                }

                item.ChangeName = FileService.CleanFileName(item.ChangeName);
                item.ChangeName = FileService.RemoveSpaces(item.ChangeName);
                item.ChangeName = FileService.FixCover(item.ChangeName);
            }

            var changedItems = FileService.ModifyStatus(items);

            FileService.ChangeFileNames(changedItems);
            
            FileService.WriteReport(changedItems);

            Console.WriteLine("Finished...");
        }
    }
}
