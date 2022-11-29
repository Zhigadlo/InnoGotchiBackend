namespace InnnoGotchi.DAL.Entities
{
    public class Farm
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public List<Pet> Pets { get; set; }
        public List<User> Сollaborators { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
