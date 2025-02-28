using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rest_api_veterinaria.AuthModule;

namespace rest_api_veterinaria.Controllers;


[ApiController]
[Route("auth")]
[Authorize]
public class VeterinariaAuthController : ControllerBase
{

    private readonly ILogger<VeterinariaAuthController> _logger;
    private readonly JWTHelper _authHelper;

    public VeterinariaAuthController(ILogger<VeterinariaAuthController> logger, JWTHelper helper)
    {
        _logger = logger;
        _authHelper = helper;
    }

    // Obtener los datos de la persona autenticada
    [AllowAnonymous]
    [HttpGet("authentication")]
    public IActionResult GetMe([FromQuery] string user, [FromQuery] string password)
    {
        if (user == "admin" && password == "admin")
        {
            var succesResponse = _authHelper.GenerateJwtToken(user);
            return Ok(succesResponse);
        }
        else if (user == "user" && password == "user")
        {
            var succesResponse = _authHelper.GenerateJwtToken(user);
            return Ok(succesResponse);
        }
        else
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}
