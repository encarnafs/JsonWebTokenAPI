using JsonWebTokenAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace JsonWebTokenAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthorizationController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestAuthorizationController(IConfiguration configuration)
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

            return Ok( new { Id = userId, deserializeUser });
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult CallWithAnonymous() => Ok("Has realizado una llamada anónima");
    }
}
