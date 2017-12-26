namespace MashZavod.Models.MissionsModels
{
    /// <summary>
    /// Модель, с помощью которой происходит передача необходимых для добавления параметров в контроллер
    /// </summary>
    public class MissionModelAdd
    {
        /// <summary>
        /// Логин получателя поручения
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Текст поручения
        /// </summary>
        public string Text { get; set; }
    }
}