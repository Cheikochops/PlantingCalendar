namespace PlantingCalendar.Models
{
    public class SeedDetailModel : SeedItemModel
    {
        public string Description { get; set; }

        public string ExpiryDate { get; set; }

        public List<SeedAction> Actions { get; set; } = new List<SeedAction>();

        public SeedAction SowAction { get; set; }

        public SeedAction HarvestAction { get; set; }
    }

    public class SeedAction
    {
        public string ActionName { get; set; }

        public string ActionDescription { get; set; }

        public ActionTypeEnum ActionType { get; set; }

        public long? ActionId {  get; set; }

        public char? DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public string StartDateMonth { get; set; }

        public string StartDateDay { get; set; }

        public string EndDateMonth { get; set; }

        public string EndDateDay { get; set; }
    }
}