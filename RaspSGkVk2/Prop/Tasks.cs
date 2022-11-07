using RaspSGkVk2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class Tasks
    {

        [Key]
        public int IdTask { get; set; }
        public char? TypeTask { get; set; }
        public List<Value> PropsValue { get; set; }
        public float? PeerId { get; set; }

        //public RaspApi Result { get; set; }
        public string? ResultText { get; set; }

    }
}
