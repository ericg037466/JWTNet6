using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTNet6.Controllers
{
    
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        { 
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> Get([FromQuery] string request)
        {
            
            return Ok();

        }
    }
}
