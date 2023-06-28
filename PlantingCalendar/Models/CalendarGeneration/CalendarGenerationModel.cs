namespace PlantingCalendar.Models
{
    public class CalendarGeneration
    {
        public string CalendarName { get; set; }

        public int Year { get; set; }

        public List<int> SeedIds { get; set; }
    }
}