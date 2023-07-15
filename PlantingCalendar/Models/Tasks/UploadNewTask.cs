namespace PlantingCalendar.Models
{
    public class UploadNewTask
    {
        public long CalendarId { get; set; }

        public List<long> Seeds { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsRanged { get; set; }

        public DateTime? RangeStartDate { get; set; }

        public DateTime? RangeEndDate { get; set; }

        public string? RepeatableType { get; set; }

        public DateTime? SingleDate { get; set;}

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool IsDisplay { get; set; }

        public string? DisplayChar { get; set; }

        public string? DisplayColour { get; set; }
    }
}