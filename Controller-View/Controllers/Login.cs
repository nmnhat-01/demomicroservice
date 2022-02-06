using Microsoft.AspNetCore.Mvc;
using TestJWT.Models;
using TestJWT.Services;

namespace Controller_View.Controllers
{
    public class Login : Controller
    {
        private IUserService _userService;
        public Login(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model, ipAddress());

            if (response == null)
            {
                Response.Cookies.Delete("Authorization");
                ModelState.AddModelError("", "Username or password is incorrect.");
                return View(model);
            }

            HttpContext.Response.Cookies.Append("Authorization", response.Token);
            setTokenCookie(response.RefreshToken);

            return Redirect("/");
        }

        // helper methods
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
