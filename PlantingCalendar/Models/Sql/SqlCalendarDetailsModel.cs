namespace PlantingCalendar.Models.Sql
{
    public class SqlCalendarDetailsModel : CalendarItemBasicModel
    {
        public int Year { get; set; }

        public string? PlantTypeName { get; set; }

        public string? PlantBreed { get; set; }

        public long? SeedId { get; set; }

        public string? TaskName { get; set; }

        public string? TaskDescription { get; set; }

        public bool IsRanged { get; set; }

        public DateTime? SetDate { get; set; }

        public DateTime? RangeStartDate { get; set; }

        public DateTime? RangeEndDate { get; set; }

        public long? TaskId { get; set; }

        public bool? IsComplete { get; set; }

        public bool? IsDisplay { get; set; }

        public string? DisplayColour { get; set; }

        public string? DisplayChar { get; set; }
    
    }
}