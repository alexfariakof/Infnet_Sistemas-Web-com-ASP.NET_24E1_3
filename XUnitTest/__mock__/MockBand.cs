using Application.Account.Dto;
using Domain.Streaming.Agreggates;
using Bogus;

namespace __mock__;
public class MockBand
{
    private static readonly Lazy<MockBand> _instance = new Lazy<MockBand>(() => new MockBand());

    public static MockBand Instance => _instance.Value;

    private MockBand() { }

    public Band GetFaker()
    {
        var mockMusics = MockMusic.Instance.GetListFaker(5);
        var mockAlbum = MockAlbum.Instance.GetFaker(mockMusics);

        var fakeBand = new Faker<Band>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Name.FullName())
            .RuleFor(b => b.Description, f => f.Lorem.Sentence())
            .RuleFor(b => b.Backdrop, f => f.Image.PlaceImgUrl())
            .RuleFor(b => b.Albums, f => new List<Album> { mockAlbum })
            .Generate();

        return fakeBand;
    }

    public BandDto GetDtoFromBand(Band band)
    {
        var fakeBandDto = new Faker<BandDto>()
            .RuleFor(b => b.Id, f => band.Id)
            .RuleFor(b => b.Name, f => band.Name)
            .RuleFor(b => b.Description, f => band.Description)
            .RuleFor(b => b.Backdrop, f => band.Backdrop)
            .RuleFor(b => b.Album, f => MockAlbum.Instance.GetDtoFromAlbum(band.Albums.FirstOrDefault()))
            .Generate();

        return fakeBandDto;
    }
    public List<Band> GetListFaker(int count)
    {
        var bandList = new List<Band>();
        for (var i = 0; i < count; i++)
        {
            bandList.Add(GetFaker());
        }
        return bandList;
    }


    public List<BandDto> GetDtoListFromBandList(IList<Band> bands)
    {
        var bandDtoList = bands.Select(GetDtoFromBand).ToList();
        return bandDtoList;
    }
}
