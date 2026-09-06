namespace SistemaGastronomico.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Usuarios;
using SistemaGastronomico.Application.Usuarios.Commands.LoginUser;
using SistemaGastronomico.Application.Usuarios.Commands.RegisterUser;

public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var nombre = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var rol = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            UsuarioId = userId,
            Nombre = nombre,
            Email = email,
            Rol = rol
        });
    }
}
