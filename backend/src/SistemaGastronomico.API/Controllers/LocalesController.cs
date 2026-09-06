namespace SistemaGastronomico.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Locales;
using SistemaGastronomico.Application.Locales.Commands.CreateLocal;
using SistemaGastronomico.Application.Locales.Commands.ToggleAperturaLocal;
using SistemaGastronomico.Application.Locales.Commands.UpdateLocal;
using SistemaGastronomico.Application.Locales.Queries.GetLocalById;
using SistemaGastronomico.Application.Locales.Queries.GetLocales;
using SistemaGastronomico.Application.Locales.Queries.GetLocalesByPropietario;
using SistemaGastronomico.Domain.Enums;

public class LocalesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LocalDto>>> GetAll(
        [FromQuery] CampusUTA? campus,
        [FromQuery] CategoriaLocal? categoria,
        [FromQuery] string? search)
    {
        var result = await Mediator.Send(new GetLocalesQuery(campus, categoria, search));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LocalDto>> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetLocalByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("mis-locales")]
    [Authorize]
    public async Task<ActionResult<List<LocalDto>>> GetMisLocales()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new GetLocalesByPropietarioQuery(userId));
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<LocalDto>> Create([FromBody] CreateLocalCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<LocalDto>> Update(Guid id, [FromBody] UpdateLocalCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("El ID de la ruta no coincide con el cuerpo de la solicitud.");
        }

        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/toggle-apertura")]
    [Authorize]
    public async Task<ActionResult<bool>> ToggleApertura(Guid id)
    {
        var result = await Mediator.Send(new ToggleAperturaLocalCommand(id));
        return Ok(new { localId = id, estaAbierto = result });
    }
}
