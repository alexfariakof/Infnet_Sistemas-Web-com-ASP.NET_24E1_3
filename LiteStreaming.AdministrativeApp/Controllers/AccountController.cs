using LiteStreaming.AdministrativeApp.Models;
using Application.Administrative.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application.Shared.Dto;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace LiteStreaming.AdministrativeApp.Controllers;

public class AccountController : Controller
{
    private readonly IAdminAuthService authenticationService;

    public AccountController(IAdminAuthService authenticationService) 
    {
        this.authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult SingIn(LoginDto dto)
    {
        if (ModelState is { IsValid: false })
            return View("Index");

        try
        {
            var accountDto = authenticationService.Authentication(dto);
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountDto.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, accountDto.Name));
            identity.AddClaim(new Claim(ClaimTypes.Email, accountDto.Email));
            identity.AddClaim(new Claim(ClaimTypes.Role, accountDto.PerfilType.ToString()));

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = "/account"
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties); 
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            
            if (ex is ArgumentException argEx)
            {
                ModelState.AddModelError("login_failed", argEx.Message);
            }
            else
                ViewBag.Alert = new AlertViewModel(AlertViewModel.AlertType.Danger, "Ocorreu um erro ao realizar login.");
            return View("Index");
        }
    }

    [Authorize]
    public IActionResult LogOut()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
