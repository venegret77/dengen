namespace MashZavod.Models.RemindersModels
{
    /// <summary>
    /// Модель, с помощью которой происходит передача необходимых для добавления параметров в контроллер
    /// </summary>
    public class ReminderPartialModelAdd
    {
        /// <summary>
        /// Логин получателя напоминания
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Текст напоминания
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Привязка к документу
        /// </summary>
        public int IdDocument { get; set; }
    }
}