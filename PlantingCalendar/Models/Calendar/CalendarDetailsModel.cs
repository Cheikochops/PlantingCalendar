namespace PlantingCalendar.Models
{
    public class CalendarDetailsModel
    {
        public string CalendarName { get; set; }   

        public long CalendarId {  get; set; }

        public int Year { get; set; }

        public List<Seed> Seeds { get; set; }        
    }

    public class Seed
    {
        public long Id { get; set; }

        public string PlantTypeName { get; set; }

        public string PlantBreed { get; set; }

        public List<Month> Months { get; set; }
    }

    public class Month
    {
        public string MonthName { get; set; }

        public string MonthCode { get; set; }

        public int DayCount { get; set; }

        public int Order { get; set; }

        public List<CalendarTask> Tasks { get; set; }
    }

    public class CalendarTask
    {
        public long Id { get; set; }

        public long TaskTypeId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime? TaskDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskEndDate { get; set; }

        public bool IsComplete { get; set; }

        public string DisplayColour { get; set; }

        public char DisplayChar { get; set; }
    }
}