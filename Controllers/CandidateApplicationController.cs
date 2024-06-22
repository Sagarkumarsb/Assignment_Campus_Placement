using Assignment_Campus_Placement.Dtos;
using Assignment_Campus_Placement.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CandidateApplicationController : ControllerBase
{
    private readonly ICandidateApplicationService _applicationService;

    public CandidateApplicationController(ICandidateApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitApplication([FromBody] CandidateApplicationDto applicationDto)
    {
        await _applicationService.SubmitApplicationAsync(applicationDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetApplicationById(string id)
    {
        var application = await _applicationService.GetApplicationByIdAsync(id);
        if (application == null)
            return NotFound();
        return Ok(application);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApplications()
    {
        var applications = await _applicationService.GetAllApplicationsAsync();
        return Ok(applications);
    }
}
