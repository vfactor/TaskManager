using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
  [Authorize(Policy = "RequireAdministratorRole")]
  public class AdminController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult UserRole()
    {
      return View();
    }

    public IActionResult UserManager()
    {
      return View();
    }
  }
}
