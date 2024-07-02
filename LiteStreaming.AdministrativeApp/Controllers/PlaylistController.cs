using LiteStreaming.AdministrativeApp.Controllers.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Application.Streaming.Dto;
using Microsoft.AspNetCore.Authorization;
using LiteStreaming.Application.Abstractions;

namespace LiteStreaming.AdministrativeApp.Controllers;

public class PlaylistController : UnitControllerBase<PlaylistDto>
{
    public PlaylistController(IService<PlaylistDto> playlistService): base(playlistService)  { }

    [HttpPost]
    [Authorize]
    public IActionResult Save(PlaylistDto viewModel)
    {
        if (ModelState is { IsValid: false })
            return Create();

        try
        {
            viewModel.UsuarioId = UserId;
            this.Services.Create(viewModel);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao salvar os dados da playlista.");
            return Create();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Update(PlaylistDto viewModel)
    {
        if (ModelState is { IsValid: false })
            return EditView();

        try
        {
            viewModel.UsuarioId = UserId;
            this.Services.Update(viewModel);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao atualizar a playlista { viewModel?.Name }.");
            return EditView();
        }
    }

    [Authorize]
    public IActionResult Edit(Guid Id)
    {
        try
        {
            var viewModel = this.Services.FindById(Id);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao editar os dados desta playlista.");
        }
        return IndexView();
    }

    [Authorize]
    public IActionResult Delete(PlaylistDto dto)
    {
        try
        {
            dto.UsuarioId = UserId;
            var result = this.Services.Delete(dto);
            if (result)
                ViewBag.Alert = SuccessMessage($"Playlista { dto?.Name } excluída.");
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);

            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao excluir a playlista {dto?.Name }.");
        }
        return IndexView();
    }
}