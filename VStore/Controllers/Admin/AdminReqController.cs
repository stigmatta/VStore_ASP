using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/reqs")]
[Authorize(Policy = "CookieAdminPolicy")]
public class AdminReqController : ControllerBase
{
    private readonly IListRepository<MinimumRequirement> _minReqRepo;
    private readonly IListRepository<RecommendedRequirement> _recReqRepo;

    public AdminReqController(IListRepository<MinimumRequirement> minReqRepo, IListRepository<RecommendedRequirement> recReqRepo)
    {
        _minReqRepo = minReqRepo;
        _recReqRepo = recReqRepo;
    }

    [HttpPost("add-minreq")]
    public async Task<IActionResult> AddMinReq([FromBody] MinimumRequirement newreq)
    {
        await _minReqRepo.Add(newreq);
        return Ok(new { success = true });
    }

    [HttpPost("add-recreq")]
    public async Task<IActionResult> AddRecReq([FromBody] RecommendedRequirement newreq)
    {
        await _recReqRepo.Add(newreq);
        return Ok(new { success = true });
    }

    [HttpGet("minreqs")]
    public async Task<IActionResult> GetMinReqs()
    {
        var minReqs = await _minReqRepo.GetAll();
        return Ok(minReqs);
    }

    [HttpGet("recreqs")]
    public async Task<IActionResult> GetRecReqs()
    {
        var recReqs = await _recReqRepo.GetAll();
        return Ok(recReqs);
    }
}
