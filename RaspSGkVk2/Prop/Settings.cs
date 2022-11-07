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
        public int IdSetting { get; set; }
        //Токен от группы ВК
        public string TokenVk { get; set; }
        //Тайм аут между запросами
        public int Timer { get; set; }
        // ID группы
        public long IdGroup { get; set; }
        //Лист с настройками бесед

    }
}
