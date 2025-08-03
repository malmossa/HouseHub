using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseHub.Shared.Models;
using HouseHub.Web.Data;



namespace HouseHub.Web.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var profiles = await _context.UserProfiles.ToListAsync();
            return View(profiles);
        }

        // GET: /UserProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /UserProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                userProfile.Id = Guid.NewGuid();
                userProfile.CreatedAt = DateTime.UtcNow;
                _context.Add(userProfile);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: UserProfiles/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);

            if (profile != null)
            {
                _context.UserProfiles.Remove(profile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(m => m.Id == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

    }
}
