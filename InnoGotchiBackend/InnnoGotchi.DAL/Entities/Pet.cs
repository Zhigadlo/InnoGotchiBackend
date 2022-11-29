namespace InnnoGotchi.DAL.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Appearance Appearance { get; set; }
        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
