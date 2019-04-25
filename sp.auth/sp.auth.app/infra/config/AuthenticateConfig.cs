namespace sp.auth.app.infra.config
{
    public class AuthenticateConfig
    {
        public int sessionExpiredInSec { get; set; }
        public int renewExpiredInSec { get; set; }
    }
}