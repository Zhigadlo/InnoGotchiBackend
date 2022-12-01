namespace InnoGotchi.BLL.DTO
{
    public class FarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public List<PetDTO> Pets { get; set; }
        public List<UserDTO> Сollaborators { get; set; }

        public int OwnerId { get; set; }
        public UserDTO Owner { get; set; }

        public double GetAverageHappinessDays() => Pets.Average(p => p.GetHappinessDaysCount());
        public double GetAveragePetsAge() => Pets.Average(p => p.GetAge());
        
        public double GetAverageFeedingPeriod() => Pets.Average(p => p.GetAverageFeedingPeriod());
        public double GetAverageDrinkingPeriod() => Pets.Average(p => p.GetAverageDrinkingPeriod());
        
        public int GetAlivePetsCount() => Pets.Count(p => p.GetPetState() != PetState.Dead);
        public int GetDeadPetsCount() => Pets.Count(p => p.GetPetState() == PetState.Dead);
    }
}
