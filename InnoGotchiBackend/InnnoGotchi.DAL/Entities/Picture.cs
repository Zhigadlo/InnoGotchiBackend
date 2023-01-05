namespace InnnoGotchi.DAL.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
