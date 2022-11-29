namespace InnoGotchi.BLL.DTO
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FeedingCount { get; set; }
        public int DrinkingCount { get; set; }

        public AppearanceDTO Appearance { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public DateTime FirstHappinessDate { get; set; }

        public int FarmId { get; set; }
        public FarmDTO Farm { get; set; }


    }
}
