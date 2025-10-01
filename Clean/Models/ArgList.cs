using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Models
{
    public class ArgList
    {
        public ArgList(bool subFolder, bool properCase)
        {
            this.SubFolder = subFolder;
            this.ProperCase = properCase;
        }

        public bool SubFolder { get; set; }

        public bool ProperCase { get; set; }
    }
}
