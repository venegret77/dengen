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
        public DateTime Time_comments;//дата отправки сообщения(должна быть точность до милисекунд)

        [Required]
        public string Text_comments { get; set; }//текст сообщения
    }
}