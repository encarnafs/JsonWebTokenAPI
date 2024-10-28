using JsonWebTokenAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace JsonWebTokenAPI.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthorizationWithRolesController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestAuthorizationWithRolesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult CallWithAuthorization()
        {
            //OBTENGO DATOS DE CLAIM
            var dataClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.UserData);
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            
            //DESERIALIZO DE STRING A User, LA PROPIEDAD dataClaim obtenida
            var deserializeUser = JsonSerializer.Deserialize<User>(dataClaim.Value);

            return Ok(new { Id = userId, deserializeUser });
        }
    }
}
