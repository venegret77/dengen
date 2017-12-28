using System.Collections.Generic;

namespace MashZavod.Models.RemindersModels
{
    /// <summary>
    /// Модель представления напоминаний на странице
    /// </summary>
    public class ReminderViewModel
    {
        /// <summary>
        /// Имеются ли входящие напоминания
        /// </summary>
        public bool IsReminderModels { get; set; }

        /// <summary>
        /// Список напоминаний
        /// </summary>
        public List<ReminderModel> ReminderModels { get; set; }

        /// <summary>
        /// Список пользователей для вывода
        /// </summary>
        public List<string> Users { get; set; }
    }
}