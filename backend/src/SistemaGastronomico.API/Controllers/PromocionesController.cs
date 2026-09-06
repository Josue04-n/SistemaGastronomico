namespace SistemaGastronomico.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Promociones;
using SistemaGastronomico.Application.Promociones.Commands.CreatePromocion;
using SistemaGastronomico.Application.Promociones.Commands.TogglePromocion;
using SistemaGastronomico.Application.Promociones.Queries.GetPromocionesByLocal;

public class PromocionesController : ApiControllerBase
{
    [HttpGet("local/{localId:guid}")]
    public async Task<ActionResult<List<PromocionDto>>> GetByLocal(Guid localId, [FromQuery] bool? soloActivas)
    {
        var result = await Mediator.Send(new GetPromocionesByLocalQuery(localId, soloActivas));
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PromocionDto>> Create([FromBody] CreatePromocionCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/toggle")]
    [Authorize]
    public async Task<ActionResult> Toggle(Guid id)
    {
        var activa = await Mediator.Send(new TogglePromocionCommand(id));
        return Ok(new { promocionId = id, estaActiva = activa });
    }
}
