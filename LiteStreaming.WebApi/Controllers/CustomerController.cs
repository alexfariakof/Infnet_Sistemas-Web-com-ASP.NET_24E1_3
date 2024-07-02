using Application.Streaming.Dto;
using Application.Streaming.Dto.Interfaces;
using LiteStreaming.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Abstractions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : UnitControllerBase
{
    private readonly IService<CustomerDto> _customerService;
    private readonly IPlaylistPersonalService _playlistService;

    public CustomerController(IService<CustomerDto> customerService, IPlaylistPersonalService playlistService)
    {
        _customerService = customerService;
        _playlistService = playlistService;
    }

    [HttpGet]
    [ProducesResponseType((200), Type = typeof(CustomerDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((404), Type = null)]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult FindById()
    {
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
    [ProducesResponseType((403))]
    [Authorize]
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
    [ProducesResponseType((200), Type = typeof(CustomerDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult Update(CustomerDto dto)
    {
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
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult Delete(CustomerDto dto)
    {
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
    [ProducesResponseType((401))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult FindAllPlaylist()
    {
        try
        {
            var result = this._playlistService.FindAll(UserIdentity);
            return Ok(result);
        }
        catch
        {
            return Ok(new List<PlaylistPersonalDto>());
        }
    }

    [HttpGet("MyPlaylist/{playlistId}")]
    [ProducesResponseType((200), Type = typeof(PlaylistPersonalDto))]
    [ProducesResponseType((401))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult FindByIdPlaylist([FromRoute] Guid playlistId)
    {
        try
        {
            var result = this._playlistService.FindById(playlistId);
            return Ok(result);
        }
        catch
        {
            return Ok(new List<PlaylistPersonalDto>());
        }
    }

    [HttpPost("MyPlaylist")]
    [ProducesResponseType((200), Type = typeof(PlaylistPersonalDto))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult CreatePlaylist([FromBody] PlaylistPersonalDto dto)
    {
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "POST" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage)); try
        {
            dto.CustomerId = UserIdentity;
            var result = this._playlistService.Create(dto);
            if (result is null) throw new();
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
    [ProducesResponseType((403))]
    [Authorize]

    public IActionResult UpdatePlaylist(PlaylistPersonalDto dto)
    {
        try
        {
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "PUT" }
        }), validationResults, validateAllProperties: true);

            if (!isValid)
                return BadRequest(validationResults.Select(error => error.ErrorMessage));


            dto.CustomerId = UserIdentity;
            var result = this._playlistService.Update(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("MyPlaylist/{playlistId}")]
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult DeletePlaylist([FromRoute] Guid playlistId)
    {
        var dto = new PlaylistPersonalDto { Id = playlistId };
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "DELETE" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage));

        try
        {
            dto.CustomerId = UserIdentity;
            var result = this._playlistService.Delete(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("MyPlaylist/{playlistId}/Music/{musicId}")]
    [ProducesResponseType((200), Type = typeof(bool))]
    [ProducesResponseType((400), Type = typeof(string))]
    [ProducesResponseType((403))]
    [Authorize]
    public IActionResult DeleteMusicFromPlaylist([FromRoute] Guid playlistId, [FromRoute] Guid musicId)
    {
        var dto = new PlaylistPersonalDto { Id = playlistId, Musics = { new MusicDto { Id = musicId } } };
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, serviceProvider: null, items: new Dictionary<object, object>
        {
            { "HttpMethod", "DELETE" }
        }), validationResults, validateAllProperties: true);

        if (!isValid)
            return BadRequest(validationResults.Select(error => error.ErrorMessage));

        try
        {
            var playlists = this._playlistService.FindById(playlistId);
            playlists.Musics.Remove(playlists.Musics.First(m => m.Id.Equals(musicId)));
            var result = this._playlistService.Update(playlists);
            if (result != null)
                return Ok(true);
            else
                throw new ArgumentException("Erro ao excluir m�sica da playlist.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}