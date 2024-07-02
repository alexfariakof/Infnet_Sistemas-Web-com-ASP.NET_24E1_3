using LiteStreaming.AdministrativeApp.Controllers.Abstractions;
using Application.Administrative.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LiteStreaming.Application.Abstractions;

namespace LiteStreaming.AdministrativeApp.Controllers;

[Authorize]
public class UserController : UnitControllerBase<AdminAccountDto>
{
    public UserController(IService<AdminAccountDto> administrativeAccountService) : base(administrativeAccountService) { }

    [Authorize(Roles = "Admin")]
    public override IActionResult Create()
    {
        return View(CREATE);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Save(AdminAccountDto dto)
    {
        if (ModelState is { IsValid: false })
            return this.Create();

        try
        {
            dto.UsuarioId = UserId;
            this.Services.Create(dto);
            return this.RedirectToIndexView();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao salvar os dados do usuário.");
            return this.Create();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Update(AdminAccountDto dto)
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
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao atualizar os dados do usuário {dto?.Name}.");
            return EditView();
        }
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(Guid Id)
    {
        try
        {
            var result = this.Services.FindById(Id);
            return View(result);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);

            else
                ViewBag.Alert = ErrorMessage("Ocorreu um erro ao editar os dados deste usuário.");
        }
        return IndexView();
    }


    public IActionResult Delete(AdminAccountDto dto)
    {
        try
        {
            dto.UsuarioId = UserId;
            var result = this.Services.Delete(dto);
            if (result)
                ViewBag.Alert = SuccessMessage($"Usuário {dto?.Name} excluído.");
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException argEx)
                ViewBag.Alert = WarningMessage(argEx.Message);
            else
                ViewBag.Alert = ErrorMessage($"Ocorreu um erro ao excluir o usuário {dto?.Name}.");
        }
        return IndexView();
    }
}