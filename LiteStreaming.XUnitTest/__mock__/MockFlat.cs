using Domain.Streaming.Agreggates;
using Bogus;
using Domain.Core.ValueObject;
using Application.Streaming.Dto;

namespace __mock__;
public class MockFlat
{
    private static readonly Lazy<MockFlat> _instance = new Lazy<MockFlat>(() => new MockFlat());
    private readonly Faker<Flat> _faker;

    private MockFlat()
    {
        _faker = new Faker<Flat>()
            .RuleFor(f => f.Id, f => f.Random.Guid())
            .RuleFor(f => f.Name, f => f.Commerce.ProductName())
            .RuleFor(f => f.Description, f => f.Lorem.Sentence())
            .RuleFor(f => f.Value, f => new Monetary(f.Finance.Amount()));
    }

    public static MockFlat Instance => _instance.Value;

    public Flat GetFaker()
    {
        return _faker.Generate();
    }

    public List<Flat> GetListFaker(int count = 3)
    {
        var flatList = new List<Flat>();
        for (var i = 0; i < count; i++)
        {
            flatList.Add(GetFaker());
        }
        return flatList;
    }

    public List<FlatDto> GetListDtoFaker(int count = 3)
    {
        var flatList = new List<FlatDto>();
        for (var i = 0; i < count; i++)
        {
            flatList.Add(GetFakerDto());
        }
        return flatList;
    }

    public FlatDto GetFakerDto(Guid? idUsuario = null)
    {
            var fakeFlatDto = new Faker<FlatDto>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Description, f => f.Lorem.Word())
                .RuleFor(a => a.Value, f => f.Random.Decimal())
                .Generate();
            fakeFlatDto.UsuarioId = idUsuario == null ? fakeFlatDto.Id : idUsuario.Value;
            return fakeFlatDto;
    }

    public FlatDto GetDtoFromFlat(Flat flat)
    {
        var fakeFlatDto = new Faker<FlatDto>()
            .RuleFor(b => b.Id, f => flat.Id)
            .RuleFor(b => b.Name, f => flat.Name)
            .RuleFor(b => b.Description, f => flat.Description)
            .RuleFor(b => b.Value, f => flat.Value.Value)
            .Generate();
        return fakeFlatDto;
    }

    public List<FlatDto> GetDtoListFromFlatList(IList<Flat> flats)
    {
        var flatDtoList = flats.Select(GetDtoFromFlat).ToList();
        return flatDtoList;
    }    
}