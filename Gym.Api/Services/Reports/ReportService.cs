using ClosedXML.Excel;
using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;
using Gym.Api.Contracts.Memberships;
using Gym.Api.Extensions;
using Gym.Api.Persistence;
using Gym.Api.Services.Bookings;
using Gym.Api.Services.Memberships;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OpenHtmlToPdf;
using System.Text;

namespace Gym.Api.Services.Reports;

public class ReportService(ApplicationDbContext context,
    IMembershipService _membershipService,
    IBookingService bookingService) : IReportService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMembershipService _membershipService = _membershipService;
    private readonly IBookingService _bookingService = bookingService;

    public async Task<Result<DashboardSummary>> GetDashboardSummary()
    {
        var numberOfActiveMemberships=await _membershipService.GetNumberOfActiveMemberships();

        var numberOfExpiredMemberships = await _membershipService.GetNumberOfExpiredMemberships();

        var classes=await _bookingService.GetClassesWithHighestNumberOfBookings();

        DashboardSummary dashboardSummary=new(numberOfActiveMemberships, numberOfExpiredMemberships,classes);

        return Result.Success(dashboardSummary); 
    }
    public async Task<XLWorkbook> ExportMembershipsToExcel()
    {
        var memberships = await GetMemberships();

        var woorkBook = new XLWorkbook();

      var sheet=  woorkBook.AddWorksheet("Memberships");

        string[] headerCells = { "StartDate" , "EndDate", "AutoRenewPaid" , "Status" , "Member", "Plane", "Freezes" }; 

        sheet.AddHeader(headerCells);

        for (int i = 0; i < memberships.Count; i++)
        {
            sheet.Cell(i + 2, 1).SetValue(memberships[i].StartDate);
            sheet.Cell(i + 2, 2).SetValue(memberships[i].EndDate);
            sheet.Cell(i + 2, 3).SetValue(memberships[i].AutoRenewPaid? "Yes": "No");
            sheet.Cell(i + 2, 4).SetValue(memberships[i].Status);
            sheet.Cell(i + 2, 5).SetValue(memberships[i].Member);
            sheet.Cell(i + 2, 6).SetValue(memberships[i].Plane);
            sheet.Cell(i + 2, 7).SetValue(memberships[i].NumberOfFreezes);
        }

        sheet.Format();
        return woorkBook;

    }
    public async Task<byte[]> ExportMembershipsToPDF()
    {
        var memberships = await GetMemberships();

         StringBuilder stringBuilder = new ();

        var filePath = $"{Directory.GetCurrentDirectory()}/templates/report.html";

        var html=File.ReadAllText(filePath);

        html = html.Replace("{{Title}}", "Report")
            .Replace("{{Header}}", "Memberships");


        stringBuilder.AppendLine("<table>");
        stringBuilder.AppendLine("<thead>");
        stringBuilder.AppendLine("<tr>");
        stringBuilder.AppendLine("<th>Start Date</th>");
        stringBuilder.AppendLine("<th>End Date</th>");
        stringBuilder.AppendLine("<th>AutoRenewPaid");
        stringBuilder.AppendLine("<th>Status");
        stringBuilder.AppendLine("<th>Member</th>");
        stringBuilder.AppendLine("<th>Plan</th>");
        stringBuilder.AppendLine("<th>Freezes</th>");
        stringBuilder.AppendLine("</tr>");
        stringBuilder.AppendLine("</thead>");
        stringBuilder.AppendLine("<tbody>");

        foreach (var membership in memberships)
        {
            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine($"<td>{membership.StartDate}</td>");
            stringBuilder.AppendLine($"<td>{membership.EndDate}</td>");
            stringBuilder.AppendLine($"<td>{(membership.AutoRenewPaid ? "Yes" : "No")}</td>");
            stringBuilder.AppendLine($"<td>{membership.Status}</td>");
            stringBuilder.AppendLine($"<td>{membership.Member}</td>");
            stringBuilder.AppendLine($"<td>{membership.Plane}</td>");
            stringBuilder.AppendLine($"<td>{membership.NumberOfFreezes}</td>");
            stringBuilder.AppendLine("</tr>");
        }
        stringBuilder.AppendLine("</tbody>");
        stringBuilder.AppendLine("</table>");

        html = html.Replace("{{Body}}", stringBuilder.ToString());

        var pdf = Pdf
            .From(html)
            .Content();
        return pdf;
    }
    private async Task<List<ReportMemberShipResponse>> GetMemberships()
    {
        var memberships = await _context.Memberships
           .AsNoTracking()
           .Include(ms => ms.Plan)
           .Include(ms => ms.Freezes)
           .Include(ms => ms.Member)
           .ThenInclude(m => m.User)
           .Select(ms => new ReportMemberShipResponse(
                    ms.StartDate.ToString("d"),
                    (ms.EndDate ?? DateTime.MinValue).ToString("d"),
                    ms.AutoRenewPaid,
                    ms.Status.ToString(),
                    $"{ms.Member.User.FirstName} {ms.Member.User.LastName}",
                    ms.Plan.Name,
                    ms.Freezes.Count
                   ))
               .ToListAsync();
        return memberships;
    }
}
