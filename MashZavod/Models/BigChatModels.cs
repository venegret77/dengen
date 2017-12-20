using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MashZavod.Models
{
    public class BigChatModels
    {
        [Required]
        public string LoginUs { get; set; }//логин

        [Required]
        public DateTime Time_message;//дата отправки сообщения(должна быть точность до милисекунд)

        [Required]
        public string Text_message { get; set; }//текст сообщения

        [Required]
        public int Type_chat { get; set; }//тип чата

        [Required]
        public string Name_chat { get; set; }//название чата

        [Required]
        public DateTime Time_create { get; set; }//дата создания чата

    }
}