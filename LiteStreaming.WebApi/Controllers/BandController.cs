using Application;
using Application.Streaming.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstractions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BandController : ControllerBaseTokensProps
{
    private readonly IService<BandDto> _bandService;
    public BandController(IService<BandDto> bandService)
    {
        _bandService = bandService;
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(List<BandDto>))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    public IActionResult FindAll()
    {
        try
        {
            var result = this._bandService.FindAll(UserIdentity);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{bandId}")]
    [ProducesResponseType((200), Type = typeof(BandDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    public IActionResult FindById([FromRoute] Guid bandId)
    {
        try
        {
            var result = this._bandService.FindById(bandId);
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
    [ProducesResponseType((200), Type = typeof(BandDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize(Roles = "Admin, Normal")]
    public IActionResult Create([FromBody] BandDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();
        try
        {
            var result = this._bandService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [ProducesResponseType((200), Type = typeof(BandDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize(Roles = "Admin, Normal")]
    public IActionResult Update(BandDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._bandService.Update(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete]
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize(Roles = "Admin, Normal")]
    public IActionResult Delete(BandDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._bandService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
