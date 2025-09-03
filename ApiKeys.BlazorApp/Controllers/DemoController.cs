using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeys.BlazorApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet("public")]
    public ActionResult<object> GetPublicData()
    {
        return Ok(new
        {
            Message = "This is public data - no API key required",
            Timestamp = DateTime.UtcNow,
            Data = new[] { "Item 1", "Item 2", "Item 3" }
        });
    }

    [HttpGet("secure")]
    [Authorize(Policy = "ApiKey")]
    public ActionResult<object> GetSecureData()
    {
        return Ok(new
        {
            Message = "This is secure data - API key required!",
            Timestamp = DateTime.UtcNow,
            SecretData = new[] { "Secret 1", "Secret 2", "Secret 3" },
            UserId = "demo-user"
        });
    }
}