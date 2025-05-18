using Microsoft.AspNetCore.Mvc;

namespace IoT.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public IActionResult GetTest()
        {
            return Ok("Test successful");
        }
    }   
}