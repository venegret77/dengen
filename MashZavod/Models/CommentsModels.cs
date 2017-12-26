using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MashZavod.Models
{
    public class CommentsModels
    {
        [Required]
        public string LoginUs { get; set; }//логин

        [Required]
        public DateTime Time_message;//дата отправки сообщения(должна быть точность до милисекунд)

        [Required]
        public string Text_message { get; set; }//текст сообщения
    }
}