﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Controllers;
public class ControllerBase : Controller
{
    public ControllerBase() { }
    protected Guid UserIdentity
    {
        get
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            var jwtToken = tokenHandler.ReadToken(token.Replace("Bearer ", "")) as JwtSecurityToken;
            var userId = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return new Guid(userId);
        }
    }
}
