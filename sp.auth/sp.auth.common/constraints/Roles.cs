namespace sp.auth.common.constraints
{
    public static class Roles
    {
        /// <summary>
        /// basic account which will use system
        /// </summary>
        public const string Account = "account";
        
        /// <summary>
        /// manager which allowed to disable users
        /// </summary>
        public const string Manager = "manager";
        
        /// <summary>
        /// full controll
        /// </summary>
        public const string Admin = "admin";
    }
}