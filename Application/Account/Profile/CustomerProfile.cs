using Application.Account.Dto;
using Application.Transactions.Dto;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Transactions.Agreggates;

namespace Application.Account.Profile;
public class CustomerProfile : AutoMapper.Profile
{
    public CustomerProfile() 
    {
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src 
            =>  new User() {  
                UserType = new UserType(UserTypeEnum.Customer),
                Login = new Login() { Email = src.Email, Password = src.Password }}  ))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new Phone(src.Phone)))
            .ReverseMap();

        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Login.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Number))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Addresses.FirstOrDefault()))
            //.ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Cards.FirstOrDefault(c => c.Active)))
            .AfterMap((s, d) =>
            {
                var flat = s.Signatures?.FirstOrDefault(c => c.Active)?.Flat;
                if (flat != null)
                    d.FlatId = flat.Id;
                d.Password = "********";
            });
        
        CreateMap<CardDto, Card>()            
            .ForPath(x => x.Limit.Value, m => m.MapFrom(f => f.Limit))
            .ForPath(x => x.CVV, m => m.MapFrom(f => "****"))
            .ReverseMap();

        CreateMap<AddressDto, Address>().ReverseMap();
    }
}
