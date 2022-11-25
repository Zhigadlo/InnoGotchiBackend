namespace InnnoGotchi.DAL.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime LastNormalStateTime { get; set; }
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
