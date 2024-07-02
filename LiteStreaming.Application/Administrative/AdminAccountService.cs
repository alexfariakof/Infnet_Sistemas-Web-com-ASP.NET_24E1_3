using Application.Administrative.Dto;
using Application.Administrative.Interfaces;
using Application.Shared.Dto;
using AutoMapper;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;
using EasyCryptoSalt;
using LiteStreaming.Application.Abstractions;
using Microsoft.Data.SqlClient;
using Repository.Persistency.Abstractions.Interfaces;

namespace Application.Administrative;
public class AdminAccountService : ServiceBase<AdminAccountDto, AdminAccount>, IService<AdminAccountDto>, IAdminAuthService
{
    private readonly ICrypto _crypto;
    public AdminAccountService(IMapper mapper, IRepository<AdminAccount> customerRepository, ICrypto crypto) : base(mapper, customerRepository)    
    {
        _crypto = crypto;
    }

    public override AdminAccountDto Create(AdminAccountDto dto)
    {
        IsValidPerfilUsuario(dto);

        if (this.Repository.Exists(x => x.Login != null && x.Login.Email == dto.Email))
            throw new ArgumentException("Usuário já cadastrado.");        

        var account = this.Mapper.Map<AdminAccount>(dto);
        this.Repository.Save(account);
        var result = this.Mapper.Map<AdminAccountDto>(account);
        return result;
    }

    public override AdminAccountDto FindById(Guid id)
    {
        var account = this.Repository.GetById(id);
        var result = this.Mapper.Map<AdminAccountDto>(account);
        return result;
    }

    public List<AdminAccountDto> FindAll(Guid userId)
    {
        var accounts = this.Repository.FindAll().Where(c => c.Id == userId).ToList();
        var result = this.Mapper.Map<List<AdminAccountDto>>(accounts);
        return result;
    }

    public override List<AdminAccountDto> FindAll()
    {
        var result = this.Mapper.Map<List<AdminAccountDto>>(this.Repository.FindAll());
        return result;
    }

    public override List<AdminAccountDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<AdminAccountDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override AdminAccountDto Update(AdminAccountDto dto)
    {
        IsValidPerfilUsuario(dto);
        var account = this.Mapper.Map<AdminAccount>(dto);
        this.Repository.Update(account);
        return this.Mapper.Map<AdminAccountDto>(account);
    }

    public override bool Delete(AdminAccountDto dto)
    {
        IsValidPerfilUsuario(dto);
        var account = this.Mapper.Map<AdminAccount>(dto);
        this.Repository.Delete(account);
        return true; 
    }

    public AdminAccountDto Authentication(LoginDto dto)
    {
        bool credentialsValid = false;
        var account = this.Repository.Find(u => u.Login.Email.Equals(dto.Email)).FirstOrDefault();
        if (account == null)
            throw new ArgumentException("Usuário inexistente!");
        else
        {
            credentialsValid = account is not null && !String.IsNullOrEmpty(account.Login.Password) && !String.IsNullOrEmpty(account.Login.Email) && (_crypto.IsEquals(dto.Password ?? "", account.Login.Password));
        }

        if (credentialsValid)
            return this.Mapper.Map<AdminAccountDto>(account);

        throw new ArgumentException("Usuário Inválido!");
    }
    private void IsValidPerfilUsuario(AdminAccountDto dto)
    {
        var administrador = this.Repository.Find(account => account.Id == dto.UsuarioId && account.PerfilType.Id == (int)Perfil.UserType.Admin).FirstOrDefault();
        if (administrador == null)
            throw new ArgumentException("Usuário não permitido a realizar operação.");

    }
}