using AluminumStockManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AluminumStockManager.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly MongoDbService _db;
        public DashboardModel(MongoDbService db) { _db = db; }

        public string UserName { get; set; } = "";
        public List<AluminumStockManager.Models.Stock> StockList { get; set; } = new();
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public int LowStockCount { get; set; }
        public double TotalValue { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
                return RedirectToPage("/Login");

            UserName = userName;
            StockList = await _db.GetAllStock();
            TotalItems = StockList.Count;
            TotalQuantity = StockList.Sum(s => s.Quantity);
            LowStockCount = StockList.Count(s => s.Quantity <= 5);
            TotalValue = StockList.Sum(s => s.Weight * s.Quantity * s.PricePerKg);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _db.DeleteStock(id);
            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}