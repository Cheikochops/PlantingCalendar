using System.Diagnostics.CodeAnalysis;

namespace PlantingCalendar.Models
{
    public class DataAccessSettings
    {
        public const string SectionName = "SqlDataAccess";

        [NotNull]
        public string Plantbase { get; set; }
    }
}