namespace PlantingCalendar.Models.Sql
{
    public class SqlSeedDetailsModel
    {
        public long Id { get; set; }

        public string PlantType { get; set; }

        public string Breed { get; set; }

        public string Description { get; set; }

        public string SunRequirement { get; set; }

        public string WaterRequirement { get; set; }

        public long? ActionId { get; set; }

        public string ActionType { get; set; }

        public string DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}