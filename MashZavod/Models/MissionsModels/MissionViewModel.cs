using System.Collections.Generic;

namespace MashZavod.Models.MissionsModels
{
    /// <summary>
    /// Модель представления поручений на странице
    /// </summary>
    public class MissionViewModel
    {
        /// <summary>
        /// Имеются ли входящие поручения
        /// </summary>
        public bool IsInMissionModels { get; set; }

        /// <summary>
        /// Список входящих поручений
        /// </summary>
        public List<MissionModel> InMissionModels { get; set; }

        /// <summary>
        /// Имеются ли исходящие поручения
        /// </summary>
        public bool IsOutMissionModels { get; set; }

        /// <summary>
        /// Список исходящих поручений
        /// </summary>
        public List<MissionModel> OutMissionModels { get; set; }
    }
}