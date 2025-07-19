using ClosedXML.Excel;
using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;
using Gym.Api.Contracts.Memberships;

namespace Gym.Api.Services.Reports;

public interface IReportService
{
    Task<Result<DashboardSummary>> GetDashboardSummary();
    Task<XLWorkbook> ExportMembershipsToExcel();
}
