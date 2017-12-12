using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MashZavod.Models
{
    public class BigChatModels
    {
        public string Login { get; set; }//логин
        public Nullable<DateTime> Time { get; set; }//дата
        public string Message { get; set; }//сообщение
    }
}