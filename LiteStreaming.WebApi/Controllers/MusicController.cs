using Application.Streaming.Dto;
using LiteStreaming.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstractions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicController : UnitControllerBase
{
    private readonly IService<MusicDto> _musicService;
    public MusicController(IService<MusicDto> musicService)
    {
        _musicService = musicService;
    }

    [HttpGet("search/{searchParam}")]
    [ProducesResponseType((200), Type = typeof(MusicDto[]))]
    public IActionResult Serach([FromRoute] string searchParam)
    {
        var result = this._musicService.FindAll().Where(m => m.Name.ToLower().Contains(searchParam.ToLower()));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(MusicDto[]))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin, Customer, Merchant")]
    public IActionResult FindAll()
    {
        try
        {
            var result = this._musicService.FindAll();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("{musicId}")]
    [ProducesResponseType((200), Type = typeof(MusicDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Amdin, Customer, Merchant")]
    public IActionResult FindById([FromRoute] Guid musicId)
    {
        try
        {
            var result = this._musicService.FindById(musicId);
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
    [ProducesResponseType((200), Type = typeof(MusicDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin")]
    public IActionResult Create([FromBody] MusicDto dto)
    {

        if (ModelState is { IsValid: false })
            return BadRequest();
        try
        {
            var result = this._musicService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [ProducesResponseType((200), Type = typeof(MusicDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin")]
    public IActionResult Update(MusicDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._musicService.Update(dto);
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
    [Authorize("Bearer", Roles = "Admin")]
    public IActionResult Delete(MusicDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._musicService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
