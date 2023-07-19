using JWTNet6.Filter;
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

        [HttpGet]
        [AuthorizationFilter]
        public async Task<IActionResult> Get([FromQuery] string uid)
        {
            
            return Ok();

        }
    }
}
