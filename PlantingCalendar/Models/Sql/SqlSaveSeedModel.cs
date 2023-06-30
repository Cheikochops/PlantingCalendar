namespace PlantingCalendar.Models.Sql
{
    public class SqlSaveSeedModel
    {
        public long? Id { get; set; }

        public string PlantType { get; set; }

        public string Breed { get; set; }

        public string SunRequirement { get; set; }

        public string WaterRequirement { get; set; }

        public string Description { get; set; }

        public string ExpiryDate { get; set; }

        public List<SqlSaveSeedAction> Actions { get; set; } = new List<SqlSaveSeedAction>();
    }

    public class SqlSaveSeedAction
    {
        public string ActionName { get; set; }

        public long? ActionId { get; set; }

        public ActionType ActionType { get; set; }

        public char? DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}