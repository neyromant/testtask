using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces;

namespace TestTask.UI.Controllers.v1;

[ApiController]
[Route("v1/[controller]")]
public class PingController : ControllerBase
{
    private readonly IHealthCheckService _healthCheckService;

    public PingController(IHealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
    }

    [HttpGet]
    public async Task<bool> PingAsync(CancellationToken cancellationToken)
    {
        var response = await _healthCheckService.IsAvailableAsync(cancellationToken);

        return response;
    }
}