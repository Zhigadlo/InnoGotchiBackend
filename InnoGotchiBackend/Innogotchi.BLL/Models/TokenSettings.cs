namespace InnoGotchi.BLL.Models
{
    public class TokenSettings
    {
        public int ExpireTimeHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
