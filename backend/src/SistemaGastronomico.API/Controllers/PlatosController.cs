namespace SistemaGastronomico.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Menus;
using SistemaGastronomico.Application.Menus.Commands.CreatePlato;
using SistemaGastronomico.Application.Menus.Commands.DeletePlato;
using SistemaGastronomico.Application.Menus.Commands.ToggleDisponibilidadPlato;
using SistemaGastronomico.Application.Menus.Commands.TogglePlatoDelDia;
using SistemaGastronomico.Application.Menus.Commands.UpdatePlato;
using SistemaGastronomico.Application.Menus.Queries.GetPlatosByLocal;
using SistemaGastronomico.Domain.Enums;

public class PlatosController : ApiControllerBase
{
    [HttpGet("local/{localId:guid}")]
    public async Task<ActionResult<List<PlatoDto>>> GetByLocal(
        Guid localId,
        [FromQuery] bool? soloDisponibles,
        [FromQuery] CategoriaPlato? categoria)
    {
        var result = await Mediator.Send(new GetPlatosByLocalQuery(localId, soloDisponibles, categoria));
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PlatoDto>> Create([FromBody] CreatePlatoCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<PlatoDto>> Update(Guid id, [FromBody] UpdatePlatoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("El ID de la ruta no coincide con el cuerpo de la petición.");
        }

        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeletePlatoCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Módulo de actualización rápida: Cambia la disponibilidad del plato en un solo clic.
    /// </summary>
    [HttpPatch("{id:guid}/toggle-disponible")]
    [Authorize]
    public async Task<ActionResult> ToggleDisponible(Guid id)
    {
        var estaDisponible = await Mediator.Send(new ToggleDisponibilidadPlatoCommand(id));
        return Ok(new { platoId = id, estaDisponible });
    }

    /// <summary>
    /// Módulo de actualización rápida: Marca o desmarca si el plato es especial del día.
    /// </summary>
    [HttpPatch("{id:guid}/toggle-plato-del-dia")]
    [Authorize]
    public async Task<ActionResult> TogglePlatoDelDia(Guid id)
    {
        var esPlatoDelDia = await Mediator.Send(new TogglePlatoDelDiaCommand(id));
        return Ok(new { platoId = id, esPlatoDelDia });
    }
}
