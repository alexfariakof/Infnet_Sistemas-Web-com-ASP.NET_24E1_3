using LiteStreaming.AdministrativeApp.Controllers.Abstractions;
using LiteStreaming.AdministrativeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Application.Streaming.Dto;
using LiteStreaming.Application.Core.Interfaces.Query;
using Microsoft.AspNetCore.Authorization;
using LiteStreaming.Application.Abstractions;

namespace LiteStreaming.AdministrativeApp.Controllers;

public class MusicController : UnitControllerBase<MusicDto>
{
    private readonly IFindAll<BandDto> bandService;
    private readonly IFindAll<GenreDto> genreService;
    private readonly IFindAll<AlbumDto> albumService;
    public MusicController(IService<MusicDto> musicService, IFindAll<GenreDto> genreFindAllService, IFindAll<BandDto> bandFindAllService, IFindAll<AlbumDto> albumService): base(musicService)
    {        
        this.bandService = bandFindAllService;
        this.genreService = genreFindAllService;
        this.albumService = albumService;
    }

    [Authorize]
    public override IActionResult Create()
    {
        var genres = genreService.FindAll();
        var bands = bandService.FindAll();
        var albums = albumService.FindAll();
        var viewModel = new MusicViewModel
        {
            Music = new MusicDto(),
            Bands = bands,
            Genres = genres,
            Albums = albums
        };
        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Save(MusicViewModel viewModel)
    {
        if (ModelState is { IsValid: false })
            return CreateView();

        try
        {
            viewModel.Music.UsuarioId = UserId;
            this.Services.Create(viewModel.Music);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao salvar os dados da musica.");
            return CreateView();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Update(MusicViewModel viewModel)
    {
        if (ModelState is { IsValid: false })
            return EditView();

        try
        {
            viewModel.Music.UsuarioId = UserId;
            this.Services.Update(viewModel.Music);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao atualizar a musica { viewModel.Music?.Name }.");
            return EditView();
        }
    }

    [Authorize]
    public IActionResult Edit(Guid Id)
    {
        try
        {
            var genres = genreService.FindAll();
            var bands = bandService.FindAll();
            var albums = albumService.FindAll();

            var viewModel = new MusicViewModel
            {
                Music = this.Services.FindById(Id),
                Bands = bands,
                Genres = genres,
                Albums = albums
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao editar os dados desta musica.");
        }
        return IndexView();
    }

    [Authorize]
    public IActionResult Delete(MusicDto dto)
    {
        try
        {
            dto.UsuarioId = UserId;
            var result = this.Services.Delete(dto);
            if (result)
                ViewBag.Alert = SuccessMessage($"Musica { dto?.Name } excluída.");
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);

            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao excluir a musica {dto?.Name }.");
        }
        return IndexView();
    }
}