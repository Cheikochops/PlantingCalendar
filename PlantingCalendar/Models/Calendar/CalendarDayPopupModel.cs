namespace PlantingCalendar.Models
{
    public class CalendarDayPopupModel
    {
        public string CalendarName { get; set; }

        public long CalendarId { get; set; }

        public int Year { get; set; }

        public DayInMonth DayInMonth { get; set; }

        public DateOnly Date { get; set; } 

        public Seed Seed { get; set; }

        public List<CalendarTask> Tasks { get; set; }
    }
}