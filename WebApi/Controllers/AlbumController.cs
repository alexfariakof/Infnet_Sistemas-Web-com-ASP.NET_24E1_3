using Application;
using Application.Account.Dto;
using Domain.Account.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController : ControllerBase
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
    [Authorize("Bearer")]
    public IActionResult FindAll()
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

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
    [Authorize("Bearer")]
    public IActionResult FindById([FromRoute] Guid albumId)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

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
    [Authorize("Bearer")]
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
    [Authorize("Bearer")]
    public IActionResult Update(AlbumDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

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
    [Authorize("Bearer")]
    public IActionResult Delete(AlbumDto dto)
    {
        if (UserType != UserTypeEnum.Customer) return Unauthorized();

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
