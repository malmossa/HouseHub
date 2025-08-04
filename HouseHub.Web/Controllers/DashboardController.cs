using HouseHub.Web.Data;
using HouseHub.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHub.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow.Date;
            var weekAgo = today.AddDays(-6);

            // Query total and weekly users
            var totalUsers = await _context.UserProfiles.CountAsync();
            var newUsersThisWeek = await _context.UserProfiles
                .CountAsync(u => u.CreatedAt >= weekAgo);

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

            var viewModel = new DashboardViewModel
            {
                TotalUserProfiles = totalUsers,
                NewUsersThisWeek = newUsersThisWeek,
                Labels = labels.Select(d => d.ToString("MMM d")).ToList(),
                DataPoints = labels.Select(d =>
                    dailyCounts.FirstOrDefault(dc => dc.Date == d)?.Count ?? 0
                ).ToList()
            };

            return View(viewModel);
        }

    }
}
