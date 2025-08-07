using HouseHub.Web.Data;
using HouseHub.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HouseHub.Web.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var today = DateTime.UtcNow.Date;
            var weekAgo = today.AddDays(-6);

            // Query total and weekly users
            var totalUsers = await _context.UserProfiles.CountAsync();
            var newUsersThisWeek = await _context.UserProfiles
                .CountAsync(u => u.CreatedAt >= weekAgo);
            var newUsersToday = await _context.UserProfiles
                .CountAsync(u => DateTime.SpecifyKind(u.CreatedAt.Date, DateTimeKind.Utc) == today);


            // Prepare chart data for the last 7 days
            var dailyCounts = await _context.UserProfiles
                .Where(u => u.CreatedAt >= weekAgo)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Prepare labels and data
            var labels = Enumerable.Range(0, 7)
                .Select(i => weekAgo.AddDays(i))
                .ToList();

            return new DashboardViewModel
            {
                TotalUserProfiles = totalUsers,
                NewUsersThisWeek = newUsersThisWeek,
                NewUsersToday = newUsersToday,
                Labels = labels.Select(d => d.ToString("MMM d")).ToList(),
                DataPoints = labels.Select(d =>
                    dailyCounts.FirstOrDefault(dc => dc.Date == d)?.Count ?? 0
                ).ToList()
            };
        }
    }
}
