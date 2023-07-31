namespace PlantingCalendar.Models
{
    public class UploadNewTask
    {
        public long CalendarId { get; set; }

        public List<long> Seeds { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsRanged { get; set; }

        public string? RangeStartDate { get; set; }

        public string? RangeEndDate { get; set; }

        public string? RepeatableType { get; set; }

        public string? SingleDate { get; set;}

        public string? FromDate { get; set; }

        public string? ToDate { get; set; }

        public bool IsDisplay { get; set; }

        public string? DisplayChar { get; set; }

        public string? DisplayColour { get; set; }
    }
}