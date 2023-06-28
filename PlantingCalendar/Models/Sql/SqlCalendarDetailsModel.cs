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

        public DateTime? SetTaskDate { get; set; }

        public DateTime? RangeTaskStartDate { get; set; }

        public DateTime? RangeTaskEndDate { get; set; }

        public long? TaskTypeId { get; set; }

        public long? TaskId { get; set; }

        public bool? IsComplete { get; set; }

        public string? TaskDisplayColour { get; set; }

        public string? TaskDisplayChar { get; set; }
    
    }
}