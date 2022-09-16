using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2
{
    [Serializable]
    public class Book
    {
        public long Id { get; set; }
        public string Word { get; set; }
        public string Value { get; set; }

    }
}
