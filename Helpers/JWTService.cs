using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Messenger.Helpers
{
    public class JWTService
    {
        private string secureKey = "f3fdsfe4gth5k3jr434343rtrfef55h";

        public string Generate(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentionals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentionals);

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));
            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var jwtHundler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            jwtHundler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false

            }, out SecurityToken validToken);

            return (JwtSecurityToken) validToken;
        }
    }
}
