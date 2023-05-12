using ERPServices.Identity.Data.ValueObjects;
using ERPServices.Identity.Domain;
using ERPServices.Identity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERPServices.Identity.Helper
{
    public class JwtAuthHelper
    {
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        public JwtAuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration.GetValue<string>("JWT:SecretKey") ?? "";
        }
        public UserAuthenticatedVO Generate(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);

            var expires = DateTime.UtcNow.AddHours(10);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email  ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? "")
                }),
                Expires = expires,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenFactory = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(tokenFactory);

            var userVO = new UserAuthenticatedVO
            {
                Token = token,
                ExpireAt = expires,
                Email = user.Email ?? ""
            };

            return userVO;
        }
    }
}
