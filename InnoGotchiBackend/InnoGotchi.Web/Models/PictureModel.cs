namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// Picture model that contains data from view
    /// </summary>
    public class PictureModel
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
