using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using TaskManager.Models;

namespace TaskManager.Controllers
{
  public class AccountController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {

      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpGet, AllowAnonymous]
    public IActionResult Register()
    {
      var model = new UserRegistration();
      return View(model);
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register(UserRegistration request)
    {
      if (!ModelState.IsValid)
      {        
        return RegistrationError(request, new IdentityError[]{ new IdentityError() { Code = "message", Description = "Model is not valid" } }, ModelState);
      }

      var userCheck = await _userManager.FindByEmailAsync(request.Email);
      if (userCheck != null)
      {        
        return RegistrationError(request, new IdentityError[] { new IdentityError() { Code = "message", Description = "Emaild already exists" } }, ModelState);
      }
      
      var result = await _userManager.CreateAsync(new IdentityUser
      {
        UserName = request.Email,
        NormalizedUserName = request.Email,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
      }, request.Password);

      return (result.Succeeded) ? RedirectToAction("Login") : RegistrationError(request, result.Errors, ModelState);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
      var model = new UserLogin();
      return View(model);
    }

    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("login", "account");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLogin request)
    {
      if (!ModelState.IsValid)
      {
        return LoginError(request, new IdentityError[] { new IdentityError() { Code = "message", Description = "Model is not valid" } }, ModelState);
      }

      var user = await _userManager.FindByEmailAsync(request.Email);
      if(user != null && !user.EmailConfirmed)
      {
        return LoginError(request, new IdentityError[] { new IdentityError() { Code = "message", Description = "Email not confirmed yet" } }, ModelState);
      }

      if (await _userManager.CheckPasswordAsync(user, request.Password) == false)
      {
        return LoginError(request, new IdentityError[] { new IdentityError() { Code = "message", Description = "Invalid credentials" } }, ModelState);
      }

      var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, true);

      if (result.Succeeded)
      {
        var userRole = await _userManager.GetRolesAsync(user);
        await _userManager.AddClaimAsync(user, new Claim("UserRole", userRole[0]));

        return RedirectToAction("Index","ToDoes");
      }
      else if (result.IsLockedOut)
      {
        return View("AccountLocked");
      }
      else
      {
        return LoginError(request, new IdentityError[] { new IdentityError() { Code = "message", Description = "Invalid login attempt" } }, ModelState);        
      }
    }
   
    private IActionResult RegistrationError(UserRegistration request, IEnumerable<IdentityError> errors, ModelStateDictionary modelState)
    {
      foreach (var err in errors)
      {
        modelState.AddModelError(err.Code, err.Description);
      }

      return View(request);
    }
    private IActionResult LoginError(UserLogin request, IEnumerable<IdentityError> errors, ModelStateDictionary modelState)
    {
      foreach(var err in errors)
      {
        modelState.AddModelError(err.Code, err.Description);
      }

      return View(request);
    }
  }
}
