using Microsoft.AspNetCore.Mvc;
using Yakify.Api.Models;
using Yakify.Api.Services;

namespace Yakify.Api.Controllers;

[ApiController]
public class AdminController: ControllerBase
{
    /// <summary>
    /// Loads a new herd into the web shop. The web shop gets reset into its initial state.
    /// </summary>
    /// <param name="dto">The herd to load</param>
    /// <param name="herdService"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="205">Succesfully loaded the herd</response>
    [HttpPost]
    [Route("/yak-shop/load")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    public async Task<IActionResult> LoadHerd(
        [FromBody] CreateHerdDto dto,
        [FromServices] HerdService herdService,
        CancellationToken cancellationToken)
    {
        await herdService.LoadNewHerd(dto, cancellationToken);
        return StatusCode(StatusCodes.Status205ResetContent);
    }

    /// <summary>
    /// Gets the status of the current herd at the requested day, or an empty herd if there is none.
    /// </summary>
    /// <param name="day">The day at which to view the herd</param>
    /// <param name="herdService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/yak-shop/herd/{day}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HerdStatusDto))]
    public async Task<IActionResult> HerdStatus(
        [FromRoute] ushort day,
        [FromServices] HerdService herdService,
        CancellationToken cancellationToken)
    {
        return Ok(await herdService.GetHerdStatus(day, cancellationToken));
    }
}