using Application;
using Application.Account.Dto;
using Domain.Account.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IService<CustomerDto> _customerService;
    private readonly IService<PlaylistPersonalDto> _playlistService;

    public CustomerController(IService<CustomerDto> customerService, IService<PlaylistPersonalDto> playlistService)
    {
        _customerService = customerService;
        _playlistService = playlistService;
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(CustomerDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [Authorize("Bearer")]
    public IActionResult FindById()
    {
        if (UserType != UserTypeEnum.Customer)  return Unauthorized();

        try
        {
            var result = this._customerService.FindById(UserIdentity);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]    
    [ProducesResponseType((200), Type = typeof(CustomerDto))]
    [ProducesResponseType((400), Type = typeof(string))]    
    public IActionResult Create([FromBody] CustomerDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._customerService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize("Bearer")]
    [ProducesResponseType((200), Type = typeof(CustomerDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    public IActionResult Update(CustomerDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            dto.Id = UserIdentity;
            var result = this._customerService.Update(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize("Bearer")]
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]

    public IActionResult Delete(CustomerDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            dto.Id = UserIdentity;
            var result = this._customerService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("MyPlaylist")]
    [ProducesResponseType((200), Type = typeof(List<PlaylistPersonalDto>))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [Authorize("Bearer")]
    public IActionResult FindAllPlaylist()
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        try
        {
            var result = this._playlistService.FindAll(UserIdentity);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("MyPlaylist/{playlistId}")]
    [ProducesResponseType((200), Type = typeof(PlaylistPersonalDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [Authorize("Bearer")]
    public IActionResult FindByIdPlaylist([FromRoute] Guid playlistId)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        try
        {
            var result = this._playlistService.FindById(playlistId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("MyPlaylist")]
    [ProducesResponseType((200), Type = typeof(PlaylistPersonalDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [Authorize("Bearer")]
    public IActionResult CreatePlaylist([FromBody] PlaylistPersonalDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "POST" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage)); try
        {
            dto.CustumerId = UserIdentity;
            var result = this._playlistService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("MyPlaylist")]
    [ProducesResponseType((200), Type = typeof(PlaylistPersonalDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [Authorize("Bearer")]
    public IActionResult UpdatePlaylist(PlaylistPersonalDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "PUT" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage));

        try
        {
            dto.CustumerId = UserIdentity;
            var result = this._playlistService.Update(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("MyPlaylist")]
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]
    [Authorize("Bearer")]
    public IActionResult DeletePlaylist(PlaylistPersonalDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "DELETE" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage));

        try
        {
            dto.CustumerId = UserIdentity;
            var result = this._playlistService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}