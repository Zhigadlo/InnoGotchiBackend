using System.Text.Json.Serialization;

namespace InnoGotchi.BLL.DTO
{
    public class FarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public List<PetDTO> Pets { get; set; } = new List<PetDTO>();

        public int OwnerId { get; set; }

        [JsonIgnore]
        public UserDTO Owner { get; set; }

        public double GetAverageHappinessDays => Pets.Count() == 0 ? 0 : Pets.Average(p => p.HappinessDaysCount);
        public double GetAveragePetsAge => Pets.Count() == 0 ? 0 : Pets.Average(p => p.GetAge());

        public double GetAverageFeedingPeriod => Pets.Count() == 0 ? 0 : Pets.Average(p => p.AverageFeedingPeriod);
        public double GetAverageDrinkingPeriod => Pets.Count() == 0 ? 0 : Pets.Average(p => p.AverageDrinkingPeriod);

        public int GetAlivePetsCount => Pets.Count(p => p.GetPetState() != PetState.Dead);
        public int GetDeadPetsCount => Pets.Count(p => p.GetPetState() == PetState.Dead);
    }
}
