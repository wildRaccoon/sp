namespace sp.auth.app.infra.config
{
    public class HashConfig
    {
        public string salt { get; set; }
        public string algorithm { get; set; }
        
        public string secret { get; set; }
    }
}