namespace PlantingCalendar.Models.Sql
{
    public class SqlSaveNewTaskModel
    {
        public long SeedId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRanged { get; set; }

        public DateTime? RangeStartDate { get; set; }

        public DateTime? RangeEndDate { get; set; }

        public DateTime? SetDate { get; set; }

        public bool IsDisplay { get; set; }

        public char? DisplayChar { get; set; }

        public string DisplayColour { get; set; }
    }
}