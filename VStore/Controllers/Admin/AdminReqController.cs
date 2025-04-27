using Microsoft.AspNetCore.Mvc;
using Business_Logic.Services;
using Data_Access.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

[ApiController]
[Route("api/admin/reqs")]
//[Authorize(Policy = "CookieAdminPolicy")]
public class AdminReqController : ControllerBase
{
    private readonly IRequirementsService _requirementsService;

    public AdminReqController(IRequirementsService requirementsService)
    {
        _requirementsService = requirementsService;
    }

    [HttpPost("add-minreq")]
    public async Task<IActionResult> AddMinReq([FromBody] MinimumRequirement newreq)
    {
        await _requirementsService.AddMinimumRequirement(newreq);
        return Ok(new { success = true });
    }

    [HttpPost("add-recreq")]
    public async Task<IActionResult> AddRecReq([FromBody] RecommendedRequirement newreq)
    {
        await _requirementsService.AddRecommendedRequirement(newreq);
        return Ok(new { success = true });
    }
    [HttpDelete("min/{id}")]
    public async Task<IActionResult> DeleteMinimumRequirement(Guid id)
    {
        await _requirementsService.DeleteMinimumRequirement(id);
        return Ok(new { succes = true });
    }
    [HttpDelete("rec/{id}")]
    public async Task<IActionResult> DeleteRecommendedRequirement(Guid id)
    {
        await _requirementsService.DeleteRecommendedRequirement(id);
        return Ok(new { succes = true });
    }

    [HttpGet("minreqs")]
    public async Task<IActionResult> GetMinReqs()
    {
        var minReqs = await _requirementsService.GetAllMinimumRequirements();
        return Ok(minReqs);
    }

    [HttpGet("recreqs")]
    public async Task<IActionResult> GetRecReqs()
    {
        var recReqs = await _requirementsService.GetAllRecommendedRequirements();
        return Ok(recReqs);
    }
}