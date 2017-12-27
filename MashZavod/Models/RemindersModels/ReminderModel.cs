using System;

namespace MashZavod.Models.RemindersModels
{
    /// <summary>
    /// Модель представления единичного напоминания
    /// </summary>
    public class ReminderModel
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин отправителя напоминания
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Текст напоминания
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата назначения напоминания
        /// </summary>
        public DateTime DateReminder { get; set; }
    }
}