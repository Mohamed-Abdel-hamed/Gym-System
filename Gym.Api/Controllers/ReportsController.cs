using Gym.Api.Abstractions;
using Gym.Api.Services.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    [HttpGet("")]
    public async Task<IActionResult> DownloadMemberships()
    {
        var woorkBook = await _reportService.ExportMembershipsToExcel();

        await using MemoryStream stream = new ();
        woorkBook.SaveAs (stream);

        return File(stream.ToArray(),"application/octet-stream","memberships.xlsx");
    }
}
