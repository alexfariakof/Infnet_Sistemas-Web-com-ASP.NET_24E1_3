using Application;
using Application.Account.Dto;
using Application.Streaming.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class BandController : ControllerBase
{
    private readonly IService<BandDto> _bandService;
    public BandController(IService<BandDto> bandService)
    {
        _bandService = bandService;
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(BandDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    //[Authorize("Bearer")]
    public IActionResult FindById([FromQuery] Guid bandId)
    {
        //if (UserType != UserType.Merchant) return Unauthorized();

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
    [Authorize("Bearer")]
    public IActionResult Update(BandDto dto)
    {
        if (UserType != UserType.Customer) return Unauthorized();

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
    [Authorize("Bearer")]
    public IActionResult Delete(BandDto dto)
    {
        if (UserType != UserType.Customer) return Unauthorized();

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
