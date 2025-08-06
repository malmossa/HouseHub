using HouseHub.Web.ViewModels;
using System.Threading.Tasks;

namespace HouseHub.Web.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}
