﻿using Application.Account.Dto;
using Application.Transactions.Dto;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Transactions.Agreggates;

namespace Application.Account.Profile;
public class MerchantProfile : AutoMapper.Profile
{
    public MerchantProfile()
    {
        CreateMap<MerchantDto, Merchant>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => new Login() { Email = src.Email, Password = src.Password }))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new Phone(src.Phone)))
            .ReverseMap();

        CreateMap<Merchant, MerchantDto>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Number))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Addresses.FirstOrDefault()))
            .AfterMap((s, d) =>
            {
                var flat = s.Signatures?.FirstOrDefault(c => c.Active)?.Flat;
                if (flat != null)
                    d.FlatId = flat.Id;
                d.Email = s.Login.Email;
                d.Password = "********";
            });

        CreateMap<CardDto, Card>()
            .ForPath(x => x.Limit.Value, m => m.MapFrom(f => f.Limit))
            .ForPath(x => x.CVV, m => m.MapFrom(f => "****"))
            .ReverseMap();

        CreateMap<AddressDto, Address>().ReverseMap();
    }
}
