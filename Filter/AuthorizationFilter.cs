using JWTNet6.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTNet6.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsRefreshToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var pass = context.HttpContext.Request.Headers.TryGetValue("Token", out StringValues outValue);

            if (!pass)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult();
                return;
            }

            if (outValue.ToString().Contains("Bearer") == false)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult();
                return;
            }


            try
            {
                var token = outValue.ToString().Split(" ").Last();
                var settings = context.HttpContext.RequestServices.GetRequiredService<SettingsModel>();

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = settings.GetJWTIssuser(),
                    ValidAudience = settings.GetJWTAudience(),

                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = !IsRefreshToken,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.GetJWTKey())),
                    ClockSkew = TimeSpan.Zero,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new ContentResult();
                    return;
                }

                var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                //TODO 你自己的黑名單機制

            }
            catch (Exception ex)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new ContentResult();
                return;
            }


        }


    }
}
