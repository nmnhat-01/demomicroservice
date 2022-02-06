using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using TestJWT.Services;

namespace TestJWT.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            String[] PathAccess = new String[] { "/", "/Home", "/Home/Index", "/Home/About", "/Home/Privacy", "/Login" };
            bool accept = false;
            foreach( string path in PathAccess){
                if(context.Request.Path == path)
                {
                    accept = true;
                }
            }
            if (accept)
            {
                await _next(context);
            }
            else
            {
                /*var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();*/
                var token = context.Request.Cookies["Authorization"];

                if (token != null)
                    attachUserToContext(context, userService, token);
                else
                {
                    context.Response.Redirect("/Login");
                }
                await _next(context);
            }
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwttoken = tokenHandler.ReadToken(token);
                var expDate = jwttoken.ValidTo;
                if (expDate < DateTime.UtcNow)
                {
                    var refreshToken = context.Request.Cookies["refreshToken"];
                    var response = userService.RefreshToken(refreshToken, ipAddress(context));
                    if (response == null)
                    {
                        context.Items["User"] = null;
                        return;
                    }
                    else
                    {
                        setRefreshTokenCookie(context, response.RefreshToken);
                        context.Response.Cookies.Append("Authorization", response.Token);
                        token = response.Token;
                    }
                }

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId);
                
            }
            catch
            {
                // user is not attached to context so request won't have access to secure routes
            }
        }

        private string ipAddress(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                return context.Request.Headers["X-Forwarded-For"];
            else
                return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void setRefreshTokenCookie(HttpContext context, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            context.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
