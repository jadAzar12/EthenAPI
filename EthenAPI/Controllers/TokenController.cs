using EthenAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EthenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private JwtTokenService _jwtTokenService;
        public TokenController(JwtTokenService jwtTokenService) 
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public IActionResult GenerateToken() 
        {
            try
            {
                var Token = _jwtTokenService.GenerateToken("1", "test@email.com");
                return Ok(new { token = Token });
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }
    }
}
