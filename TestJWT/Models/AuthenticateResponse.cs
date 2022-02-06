using System.Text.Json.Serialization;

namespace TestJWT.Models
{
    public class AuthenticateResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string Token { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string token, string refreshToken)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
