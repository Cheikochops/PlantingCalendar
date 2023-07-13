namespace PlantingCalendar.Models.Sql
{
    public class SqlSaveNewTaskModel
    {
        public long SeedId { get; set; }

        public List<long> Seeds { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRanged { get; set; }

        public DateTime? RangeStartDate { get; set; }

        public DateTime? RangeEndDate { get; set; }

        public RepeatableTypeEnum RepeatableType { get; set; }

        public DateTime? SingleDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}