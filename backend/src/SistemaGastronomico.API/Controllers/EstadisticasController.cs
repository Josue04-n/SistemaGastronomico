namespace SistemaGastronomico.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Estadisticas;
using SistemaGastronomico.Application.Estadisticas.Commands.RegistrarEventoEstadistica;
using SistemaGastronomico.Application.Estadisticas.Queries.GetLocalEstadisticas;

public class EstadisticasController : ApiControllerBase
{
    [HttpGet("local/{localId:guid}")]
    [Authorize]
    public async Task<ActionResult<LocalEstadisticasDto>> GetEstadisticasLocal(Guid localId)
    {
        var result = await Mediator.Send(new GetLocalEstadisticasQuery(localId));
        return Ok(result);
    }

    [HttpPost("evento")]
    public async Task<ActionResult<Guid>> RegistrarEvento([FromBody] RegistrarEventoEstadisticaCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(new { eventoId = result });
    }
}
