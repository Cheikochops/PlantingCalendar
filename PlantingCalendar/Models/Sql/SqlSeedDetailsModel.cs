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

        public ActionTypeEnum ActionType { get; set; }

        public string ActionName { get; set; }

        public string ActionDescription { get; set; }

        public bool IsDisplay { get; set; }

        public string DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}