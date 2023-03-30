namespace PlantingCalendar.Models
{
    public class SeedDetailModel : SeedItemModel
    {
        public string Description { get; set; }

        public List<SeedAction> Actions { get; set; }
    }

    public class SeedAction
    {
        public DateOnly MinDate { get; set; }

        public DateOnly MaxDate { get; set; }

        public string ActionName { get; set; }
    }
}