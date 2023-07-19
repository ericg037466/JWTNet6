namespace JWTNet6.Model
{
    public class SettingsModel
    {
        private readonly string JWTIssuser = default;
        private readonly string JWTAudience = default;
        private readonly string JWTKey = default;

        public SettingsModel(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this.JWTIssuser = configuration["Jwt:Issuer"];
            this.JWTAudience = configuration["Jwt:Audience"];
            this.JWTKey = configuration["Jwt:Key"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetJWTIssuser() => JWTIssuser;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetJWTAudience() => JWTAudience;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetJWTKey() => JWTKey;
    }
}
