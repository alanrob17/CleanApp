using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string? Name { get; set; }

        public string? ChangeName { get; set; }

        public bool Changed { get; set; }

        public string? Path { get; set; }

        public string? Extension { get; set; }
    }
}
