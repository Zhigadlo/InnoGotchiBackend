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

        public int GetAlivePetsCount(DateTime feedingPeriod, DateTime drinkingPeriod) => Pets.Count(p => p.GetPetState(feedingPeriod, drinkingPeriod) != PetState.Dead);
        public int GetDeadPetsCount(DateTime feedingPeriod, DateTime drinkingPeriod) => Pets.Count(p => p.GetPetState(feedingPeriod, drinkingPeriod) == PetState.Dead);
    }
}
