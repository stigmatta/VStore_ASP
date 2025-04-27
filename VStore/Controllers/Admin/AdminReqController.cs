using Microsoft.AspNetCore.Mvc;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO;
using AutoMapper;

[ApiController]
[Route("api/admin/reqs")]
public class AdminReqController : ControllerBase
{
    private readonly IRequirementsService _requirementsService;
    private readonly IMapper _mapper;

    public AdminReqController(IRequirementsService requirementsService,IMapper mapper)
    {
        _requirementsService = requirementsService;
        _mapper = mapper;
    }

    [HttpPost("add-minreq")]
    public async Task<IActionResult> AddMinReq([FromBody] MinimumRequirementDTO newreq)
    {
        var req = _mapper.Map<MinimumRequirement>(newreq);
        await _requirementsService.AddMinimumRequirement(req);
        return Ok(new { success = true });
    }

    [HttpPost("add-recreq")]
    public async Task<IActionResult> AddRecReq([FromBody] RecommendedRequirementDTO newreq)
    {
        var req = _mapper.Map<RecommendedRequirement>(newreq);
        await _requirementsService.AddRecommendedRequirement(req);
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