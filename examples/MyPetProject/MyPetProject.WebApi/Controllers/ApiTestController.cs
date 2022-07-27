using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyPetProject.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/test")]
public class ApiTestController : ControllerBase
{
    [HttpGet("anonymous")]
    [AllowAnonymous]
    public IActionResult TestAnonymous()
    {
        return Ok("Anonymous - OK");
    }

    [HttpGet("authorize")]
    public IActionResult TestAuthorize()
    {
        return Ok("Authorize - OK");
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult TestAdmin()
    {
        return Ok("Admin - OK");
    }
}