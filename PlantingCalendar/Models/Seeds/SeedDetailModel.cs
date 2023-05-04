namespace PlantingCalendar.Models
{
    public class SeedDetailModel : SeedItemModel
    {
        public string Description { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public List<SeedAction> Actions { get; set; } = new List<SeedAction>();
    }

    public class SeedAction
    {
        public string ActionType { get; set; }

        public long ActionId {  get; set; }

        public char DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}