namespace PlantingCalendar.Models
{
    public class SeedItemModel
    {
        public long Id { get; set; }

        public string PlantType { get; set; }

        public string Breed { get; set; }

        public string SunRequirement { get; set; }

        public string WaterRequirement {  get; set; }
    }
}