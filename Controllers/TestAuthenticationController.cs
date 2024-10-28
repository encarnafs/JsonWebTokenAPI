using JsonWebTokenAPI.Application.Services;
using JsonWebTokenAPI.Request;
using Microsoft.AspNetCore.Mvc;


namespace JsonWebTokenAPI.Controllers
{
    [Route("api/[controller]")]
    public class TestAuthenticationController: Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public TestAuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public ActionResult DoLogin([FromBody] Credentials credentials) =>
            Ok(_authenticationService.ValidateCredentials(credentials.UserName, credentials.Password));

    }
}
