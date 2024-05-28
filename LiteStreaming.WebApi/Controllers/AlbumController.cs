using Application;
using Application.Account.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstractions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController : ControllerBaseTokensProps
{
    private readonly IService<AlbumDto> _albumService;
    public AlbumController(IService<AlbumDto> albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(List<AlbumDto>))]
    [ProducesResponseType((400), Type = typeof(string))]    
    [ProducesResponseType((404), Type = null)]
    public IActionResult FindAll()
    {
        try
        {
            var result = this._albumService.FindAll(UserIdentity);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{albumId}")]
    [ProducesResponseType((200), Type = typeof(AlbumDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin, Normal, Customer")]
    public IActionResult FindById([FromRoute] Guid albumId)
    {
        try
        {
            var result = this._albumService.FindById(albumId);
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
    [ProducesResponseType((200), Type = typeof(AlbumDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin, Normal")]
    public IActionResult Create([FromBody] AlbumDto dto)
    {

        if (ModelState is { IsValid: false })
            return BadRequest();
        try
        {
            var result = this._albumService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut]
    [ProducesResponseType((200), Type = typeof(AlbumDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize("Bearer", Roles = "Admin, Normal")]
    public IActionResult Update(AlbumDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._albumService.Update(dto);
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
    [Authorize("Bearer", Roles = "Admin, Normal")]
    public IActionResult Delete(AlbumDto dto)
    {
        if (ModelState is { IsValid: false })
            return BadRequest();

        try
        {
            var result = this._albumService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}