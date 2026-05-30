using AluminumStockManager.Models;
using AluminumStockManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AluminumStockManager.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly MongoDbService _db;
        public RegisterModel(MongoDbService db) { _db = db; }

        [BindProperty]
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet() { }

        public async Task<IActionResult>
        OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var existing = await _db.GetUserByEmail(Email);
            if (existing != null)
            {
                ErrorMessage = "Email already registered. Please login.";
                return Page();
            }

            var user = new User
            {
                FullName = FullName,
                Email = Email,
                Password = Password,
                Role = "User"
            };

            await _db.CreateUser(user);
            SuccessMessage = "Account created! You can now login.";
            return Page();
        }
    }
}