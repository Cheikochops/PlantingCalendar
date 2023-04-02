namespace PlantingCalendar.Models.Sql
{
    public class SqlCalendarDetailsModel : CalendarItemBasicModel
    {
        public int Year { get; set; }

        public string PlantTypeName { get; set; }

        public string PlantBreed { get; set; }

        public long? SeedId { get; set; }

        public string? TaskName { get; set; }

        public string? TaskDescription { get; set; }

        public DateOnly? SetTaskDate { get; set; }

        public DateOnly? RangeTaskStartDate { get; set; }

        public DateOnly? RangeTaskEndDate { get; set; }

        public long? TaskTypeId { get; set; }

        public long? TaskId { get; set; }

        public bool? IsComplete { get; set; }

        public string? TaskDisplayColour { get; set; }

        public char? TaskDisplayChar { get; set; }
    }
}