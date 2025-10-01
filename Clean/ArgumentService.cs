using Clean.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean
{
    public class ArgumentService
    {
        public static ArgList GetArguments(List<string> args)
        {
            var subFolders = false;
            var properCase = false;

            if (args.Count == 1)
            {
                if (args[0].ToLowerInvariant().Contains("s"))
                {
                    subFolders = true;
                }

                if (args[0].ToLowerInvariant().Contains("p"))
                {
                    properCase = true;
                }
            }

            var argList = new ArgList(subFolders, properCase);

            return argList;
        }
    }
}
