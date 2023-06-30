namespace PlantingCalendar.Models
{
    public class GenerateCalendarModel
    {
        public string CalendarName { get; set; }

        public int CalendarYear { get; set; }

        public List<int> Seeds { get; set; }
    }

}