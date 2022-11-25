namespace InnoGotchi.BLL.DTO
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public PetState HungryLevel { get; set; }
        public PetState ThirstyLevel { get; set; }
        public int FarmId { get; set; }
        public FarmDTO Farm { get; set; }
       
    }
}
