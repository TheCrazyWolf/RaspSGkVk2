using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class Book
    {

        [Key]
        public int IdWord { get; set; }
        public string Word { get; set; }
        public string Answers { get; set; }
    }
}
