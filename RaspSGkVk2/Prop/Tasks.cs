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
        public int IdTask { get; set; }  //Инкремент
        public char? TypeTask { get; set; } //Тип расписания
        public long? PeerId { get; set; } //ID беседы
        public string? Value { get; set; } //ID препода или группы (допускается массив через ;

        public string? ResultText { get; set; } //Прошлый ответ

    }
}
