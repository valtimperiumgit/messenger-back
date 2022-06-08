using Messenger.Database;
using Messenger.Helpers;
using Messenger.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IClientRepository _repository;
        private readonly JWTService _jwtService;

        public AuthController(IClientRepository repository, JWTService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }


        [HttpGet("start")]
        public IActionResult Start()
        {
            return Ok("success");
        }
       
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] string phone)
        {
            
            Client client = await _repository.GetByPhoneAsync(phone);
            if (client == null)
                return Json("User not found");

            string token = _jwtService.Generate(client.Id);
            

            return Json(token);
        }

        
        [HttpPost("user")]
        public async Task<IActionResult> User([FromBody] string jwtToken)
        {   
            var token = _jwtService.Verify(jwtToken);
            int idUser = int.Parse(token.Issuer);
            var user = await _repository.GetByIdAsync(idUser);
            return Ok(user);
        }

    }
}
