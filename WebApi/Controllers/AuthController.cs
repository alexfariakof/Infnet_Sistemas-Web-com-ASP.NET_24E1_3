using Application.Account.Dto;
using Application.Account.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType((200), Type = typeof(AuthenticationDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    public IActionResult SignIn([FromBody] LoginDto loginDto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = _userService.Authentication(loginDto);
            if (result == null)
                return BadRequest("Erro ao realizar login!");

            return new OkObjectResult(result);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
