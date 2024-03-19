using Application.Account.Dto;
using Domain.Account.ValueObject;

namespace Application.Account.Profile;
public class AddressProfile : AutoMapper.Profile
{
    public AddressProfile() 
    {
        CreateMap<AddressDto, Address>();
        CreateMap<Address, AddressDto>()
        .AfterMap((s, d) =>
         {
             d.Zipcode = s.Zipcode;
             d.Street = s.Street;
             d.Number = s.Number;
             d.Neighborhood = s.Neighborhood;
             d.City = s.City;
             d.Country = s.Country;
             d.State = s.State;
             d.Complement = s.Complement;
         });
    }
}
