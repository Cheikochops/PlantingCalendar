namespace PlantingCalendar.Models
{
    public class UploadTaskDetails
    {
        public long TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public char DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public bool IsRanged { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskEndDate { get; set; }

        public DateTime? TaskSetDate { get; set; }
    }
}