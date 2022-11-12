using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2.Prop
{
    internal class Settings
    {

        [Key]
        public int IdSetting { get; set; } // Инкремент
        public string TokenVk { get; set; } //Токен от группы ВК
        public long IdGroup { get; set; } //ID группы
        public int Timer { get; set; } // Ожидание между запросом

    }
}
