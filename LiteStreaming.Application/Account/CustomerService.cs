using Application.Streaming.Dto;
using Application.Streaming.Interfaces;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using LiteStreaming.Application.Abstractions;
using Repository.Persistency.Abstractions.Interfaces;
using Microsoft.Data.SqlClient;

namespace Application.Streaming;
public class CustomerService : ServiceBase<CustomerDto, Customer>, IService<CustomerDto>, ICustomerService
{
    private readonly IRepository<Flat> _flatRepository;

    public CustomerService(IMapper mapper, IRepository<Customer> customerRepository, IRepository<Flat> flatRepository) : base(mapper, customerRepository)
    {
        _flatRepository = flatRepository;
    }

    public override CustomerDto Create(CustomerDto dto)
    {
        if (this.Repository.Exists(x => x.User.Login != null && x.User.Login.Email == dto.Email))
            throw new ArgumentException("Usuário já existente na base.");


        Flat flat = this._flatRepository.GetById(dto.FlatId);

        if (flat == null)
            throw new ArgumentException("Plano não existente ou não encontrado.");

        Card card = this.Mapper.Map<Card>(dto.Card);
        card.CardBrand = CreditCardBrand.IdentifyCard(card.Number);

        Customer customer = new() {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CPF = dto.CPF,
            Birth = dto.Birth,
            Phone = dto.Phone,
            User =
            {
                Login = new()
                {
                    Email = dto.Email ?? "",
                    Password = dto.Password ?? ""
                },
                PerfilType = PerfilUser.UserType.Customer
            }
        };
        
        Address address = this.Mapper.Map<Address>(dto.Address);
        
        customer.CreateAccount(customer, address, flat, card);
        
        this.Repository.Save(customer);
        var result = this.Mapper.Map<CustomerDto>(customer);

        return result;
    }
    public override CustomerDto FindById(Guid id)
    {
        var customer = this.Repository.GetById(id);
        var result = this.Mapper.Map<CustomerDto>(customer);
        return result;
    }

    public List<CustomerDto> FindAll(Guid userId)
    {
        var customers = this.Repository.FindAll().Where(c => c.Id == userId).ToList();
        var result = this.Mapper.Map<List<CustomerDto>>(customers);
        return result;
    }

    public override List<CustomerDto> FindAll()
    {
        var customers = this.Repository.FindAll().ToList();
        var result = this.Mapper.Map<List<CustomerDto>>(customers);
        return result;
    }

    public override List<CustomerDto> FindAllOrdered(string serachParams = null, string orderProperty = null, SortOrder sortOrder = 0)
    {
        var result = this.Mapper.Map<List<CustomerDto>>(this.Repository.FindAllOrdered(serachParams, orderProperty, sortOrder));
        return result;
    }

    public override CustomerDto Update(CustomerDto dto)
    {
        var customer = this.Mapper.Map<Customer>(dto);
        this.Repository.Update(customer);
        return this.Mapper.Map<CustomerDto>(customer);
    }
    public override bool Delete(CustomerDto dto)
    {
        var customer = this.Mapper.Map<Customer>(dto);
        this.Repository.Delete(customer);
        return true; 
    }
}