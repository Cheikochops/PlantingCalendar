namespace PlantingCalendar.Models
{
    public class Month
    {
        public string MonthName { get; set; }

        public string MonthCode { get; set; }

        public int DayCount { get; set; }

        public int Order { get; set; }

        public List<DayInMonth> Days { get; set; }
    }
}