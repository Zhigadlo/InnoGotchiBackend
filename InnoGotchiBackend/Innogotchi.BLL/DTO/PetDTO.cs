using System.Text.Json.Serialization;

namespace InnoGotchi.BLL.DTO
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FeedingCount { get; set; } = 1;
        public int DrinkingCount { get; set; } = 1;

        public string Appearance { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime DeathTime { get; set; }

        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public DateTime FirstHappinessDate { get; set; }

        public int FarmId { get; set; }
        [JsonIgnore]
        public FarmDTO Farm { get; set; }

        public void Feed()
        {
            LastFeedingTime = DateTime.UtcNow;
            FeedingCount++;
        }
        public void Drink()
        {
            LastDrinkingTime = DateTime.UtcNow;
            DrinkingCount++;
        }
    }
}
