using System;

namespace MashZavod.Models.MissionsModels
{
    /// <summary>
    /// Модель представления единичного поручения
    /// </summary>
    public class MissionModel
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин отправителя / получателя, в зависимости от контекста
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Текст поручения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата назначения поручения
        /// </summary>
        public DateTime DateMission { get; set; }

        /// <summary>
        /// Выполнил ли получатель поручение?
        /// </summary>
        public bool Done { get; set; }
    }
}