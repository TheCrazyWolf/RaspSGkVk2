using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspSGkVk2
{
    [Serializable]
    public class ListAdmins
    {
        // Инкримент
        public int Id { get; set; }
        //Id Пользователя
        public long Value { get; set; }
    }
}
