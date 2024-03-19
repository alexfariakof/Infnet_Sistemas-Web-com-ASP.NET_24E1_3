using Application.Account.Dto;
using Domain.Streaming.Agreggates;
using Bogus;

namespace __mock__;
public class MockAlbum
{
    private static readonly Lazy<MockAlbum> _instance = new Lazy<MockAlbum>(() => new MockAlbum());

    public static MockAlbum Instance => _instance.Value;

    private MockAlbum() { }

    public Album GetFaker(IList<Music> musics = null)
    {
        if (musics == null)
            musics = MockMusic.Instance.GetListFaker(3);

        var fakeAlbum = new Faker<Album>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Name, f => f.Commerce.ProductName())
            .RuleFor(a => a.Musics, f => musics)
            .RuleFor(a => a.Flats, new List<Flat>())

            .Generate();

        return fakeAlbum;
    }

    public List<Album> GetListFaker(int count)
    {
        var albumList = new List<Album>();

        for (var i = 0; i < count; i++)
        {
            albumList.Add(GetFaker());
        }

        return albumList;
    }

    public AlbumDto GetDtoFromAlbum(Album album)
    {
        var albumDto = new AlbumDto
        {
            Id = album.Id,
            Name = album.Name,
            FlatId = Guid.NewGuid(),
            Musics = MockMusic.Instance.GetDtoListFromMusicList(album.Musics)
        };

        return albumDto;
    }

    public List<AlbumDto> GetDtoListFromAlbumList(IList<Album> albums)
    {
        var albumDtoList = new List<AlbumDto>();

        foreach (var album in albums)
        {
            var albumDto = GetDtoFromAlbum(album);
            albumDtoList.Add(albumDto);
        }

        return albumDtoList;
    }
}
