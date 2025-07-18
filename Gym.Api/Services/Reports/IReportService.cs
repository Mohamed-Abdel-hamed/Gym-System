using Gym.Api.Abstractions;
using Gym.Api.Contracts.Dashboards;

namespace Gym.Api.Services.Reports;

public interface IReportService
{
    Task<Result<DashboardSummary>> GetDashboardSummary();
}
