namespace PlantingCalendar.Models
{
    public class UploadTaskDetails
    {
        public string TaskName { get; set; }

        public string? TaskDescription { get; set; }

        public bool IsDisplay { get; set; }

        public string? DisplayChar { get; set; }

        public string? DisplayColour { get; set; }

        public bool IsRanged { get; set; }

        public string? TaskStartDate { get; set; }

        public string? TaskEndDate { get; set; }

        public string? TaskSetDate { get; set; }
    }
}