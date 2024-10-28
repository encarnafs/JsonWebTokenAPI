using JsonWebTokenAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JsonWebTokenAPI.Application.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private List<User> _listUsers =>
            new() { new User() { Id = 1, FirstName = "Encarna", LastName = "Fernandez", UserName = "encarnaf", Password = "123456" } };

        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration) => _configuration =  configuration;
   

        public string ValidateCredentials(string userName, string password)
        {
            //USER VALIDATION
            var user = _listUsers.Find(user => user.UserName == userName && user.Password == password);

            if (user == null) throw new System.Exception("Usuario inválido. Introduzca credenciales válidas");

            //CLAIMS.PROPERTIES
            var claims = new List<Claim>() {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),         //USER IDENTIFICATION
                new Claim (ClaimTypes.Name, user.UserName),                        //USER NAME
                new Claim (ClaimTypes.UserData, JsonSerializer.Serialize(user)),   //SERIALIZED USER DATA
                new Claim (ClaimTypes.Role, "Admin")                               //USER ROLE
            };

            //BUILD TOKEN SIGNING CREDENTIAL
            var privateKey = _configuration.GetValue<string>("Authentication:JWT:Key");             //PRIVATE KEY OF APPSETTINGS.JSON
            var simetricSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));  //SIMETRIC KEY
            var signingCredentials = new SigningCredentials(simetricSigningKey, SecurityAlgorithms.HmacSha256Signature);//SIGNATURE CREDENTIAL

            //TOKEN DESCRIPTOR
            var expiration = _configuration.GetValue<int>("Authentication:JWT:ExpirationMinutes");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiration),
                SigningCredentials = signingCredentials
            };

            //TOKEN CREATION
            var tokenHandler = new JwtSecurityTokenHandler();       //JWT TOKEN MANAGER
            var token = tokenHandler.CreateToken(tokenDescriptor);  //SECURITY TOKEN CREATOR

            return tokenHandler.WriteToken(token);                  //SERIALIZE SECURITY TOKEN TO TEXT STRING
        }
    }
}
