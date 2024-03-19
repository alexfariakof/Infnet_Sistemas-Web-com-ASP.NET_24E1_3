using Application.Transactions.Dto;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;

namespace Application.Transactions.Profile;
public class CardProfile : AutoMapper.Profile
{
    public CardProfile()
    {
        CreateMap<CardDto, Card>()
            .ForMember(dest => dest.Validate, opt => opt.MapFrom(src => new ExpiryDate(src.Validate.Value)))
            .ReverseMap();

        CreateMap<Card, CardDto>()
            .ForMember(x => x.CVV, m => m.Ignore())
            .AfterMap((s, d) =>
            {
                d.Number = s.Number;
                d.Limit = s.Limit;
                d.Validate = s.Validate.Value;
                d.CVV = "*************";
            });

        CreateMap<ExpiryDate, DateTime>().ConvertUsing(src => src.Value);
    }
}