namespace PlantingCalendar.Models
{
    public class CalendarItemModel : CalendarItemBasicModel
    {
        public int Year { get; set; }

        public List<MonthModel> Month { get; set; }
    }
}