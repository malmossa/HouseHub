namespace HouseHub.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUserProfiles { get; set; }
        public int NewUsersThisWeek { get; set; }

        public List<string> Labels { get; set; } = new();
        public List<int> DataPoints { get; set; } = new();
    }
}

