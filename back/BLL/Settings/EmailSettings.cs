namespace BLL.Settings
{
    public class EmailSettings
    {
        public string PublicApiBaseUrl { get; set; } = "http://localhost:5293";
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "rasputinillya08@gmail.com";
        public string Password { get; set; } = "auwkqbqveipaewop";
        public string FromEmail { get; set; } = "rasputinillya08@gmail.com";
        public string FromName { get; set; } = "SteamClone";
        public bool EnableSsl { get; set; } = true;
    }
}
