using RaspSGkVk2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2
{
    [Serializable]
    public class SettingsVk
    {
        public int IdTask { get; set; }
        public char TypeTask { get; set; }
        public string Value { get; set; }
        public string PeerId { get; set; }

        public RaspApi Result;
        public string ResultText;

    }
}
