namespace PlantingCalendar.Models
{
    public class UploadSeedDetailModel 
    {
        public long? Id { get; set; }

        public string PlantType { get; set; }

        public string Breed { get; set; }

        public string SunRequirement { get; set; }

        public string WaterRequirement { get; set; }

        public string Description { get; set; }

        public string ExpiryDate { get; set; }

        public List<UploadSeedAction> Actions { get; set; } = new List<UploadSeedAction>();

        public UploadSeedAction SowAction { get; set; }

        public UploadSeedAction HarvestAction { get; set; }
    }

    public class UploadSeedAction
    {
        public string ActionName { get; set; }

        public long? ActionId {  get; set; }

        public char? DisplayChar { get; set; }

        public string DisplayColour { get; set; }

        public string StartDateMonth { get; set; }

        public string StartDateDay { get; set; }

        public string EndDateMonth { get; set; }

        public string EndDateDay { get; set; }
    }
}