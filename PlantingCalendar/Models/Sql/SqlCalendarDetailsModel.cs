namespace PlantingCalendar.Models.Sql
{
    public class SqlCalendarDetailsModel : CalendarItemBasicModel
    {
        public int Year { get; set; }

        public string? TaskTypeName { get; set; }

        public string? TaskTypeDescription { get; set; }

        public DateOnly? TaskDate { get; set; }

        public DateOnly? TaskStartDate { get; set; }

        public DateOnly? TaskEndDate { get; set; }

        public long? TaskTypeId { get; set; }

        public long? TaskId { get; set; }    

        public bool? IsComplete { get; set; }

        public string? DisplayColour { get; set; }

        public char? DisplayChar { get; set; }
    }
}