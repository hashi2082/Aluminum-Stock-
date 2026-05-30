using AluminumStockManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AluminumStockManager.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MongoDbService _db;
        public LoginModel(MongoDbService db) { _db = db; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet() { }

        public async Task<IActionResult>
        OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _db.GetUserByEmail(Email);

            if (user == null || user.Password != Password)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Session mein user info save karo
            HttpContext.Session.SetString("UserId", user.Id ?? "");
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToPage("/Dashboard");
        }
    }
}