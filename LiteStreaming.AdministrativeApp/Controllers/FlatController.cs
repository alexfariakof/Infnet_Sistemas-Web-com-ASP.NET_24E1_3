using LiteStreaming.AdministrativeApp.Controllers.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Application.Streaming.Dto;
using Microsoft.AspNetCore.Authorization;
using LiteStreaming.Application.Abstractions;

namespace LiteStreaming.AdministrativeApp.Controllers;

public class FlatController : UnitControllerBase<FlatDto>
{
    public FlatController(IService<FlatDto> services): base(services)
    {
    }

    [HttpPost]
    [Authorize]
    public IActionResult Save(FlatDto dto)
    {
        if (ModelState is { IsValid: false })
            return Create();

        try
        {
            dto.UsuarioId = UserId;
            this.Services.Create(dto);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argSaveEx)
                ViewBag.Alert = WarningMessage(argSaveEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao salvar os dados do plano.");
            return Create();
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Update(FlatDto dto)
    {
        if (ModelState is { IsValid: false })
            return EditView();

        try
        {
            dto.UsuarioId = UserId;
            this.Services.Update(dto);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argUpdateEx)
                ViewBag.Alert = WarningMessage(argUpdateEx.Message);
            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao atualizar o plano { dto?.Name }.");
            return EditView();
        }
    }

    [Authorize]
    public IActionResult Edit(Guid Id)
    {
        try
        {
            var result = this.Services.FindById(Id);
            return View(result);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEditEx)
                ViewBag.Alert = WarningMessage(argEditEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao editar os dados deste plano.");
        }
        return IndexView();
    }

    [Authorize]
    public IActionResult Delete(FlatDto dto)
    {
        try
        {
            dto.UsuarioId = UserId;
            var result = this.Services.Delete(dto);
            if (result)
                ViewBag.Alert = SuccessMessage($"Plano { dto?.Name } excluído.");
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argDeleteEx)
                ViewBag.Alert = WarningMessage(argDeleteEx.Message);

            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao excluir o plano { dto?.Name }.");
        }
        return IndexView();
    }
}