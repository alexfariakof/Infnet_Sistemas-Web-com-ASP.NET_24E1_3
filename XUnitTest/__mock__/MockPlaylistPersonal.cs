using Application.Account.Dto;
using Bogus;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;

namespace __mock__;
public sealed class MockPlaylistPersonal
{
    private static readonly Lazy<MockPlaylistPersonal> _instance = new Lazy<MockPlaylistPersonal>(() => new MockPlaylistPersonal());

    private MockPlaylistPersonal() { }

    public static MockPlaylistPersonal Instance => _instance.Value;

    public PlaylistPersonal GetFaker(List<Music> musics = null)
    {
        if (musics == null) musics = new List<Music>();

        var fakePlaylist = new Faker<PlaylistPersonal>()
            .RuleFor(p => p.Customer, MockCustomer.Instance.GetFaker())
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Musics, f => musics)
            .Generate();

        return fakePlaylist;
    }

    public List<PlaylistPersonal> GetListFaker(int count, List<Music> musics = null)
    {
        var playlistList = new List<PlaylistPersonal>();
        for (var i = 0; i < count; i++)
        {
            if (musics == null)
                playlistList.Add(GetFaker());
            else
                playlistList.Add(GetFaker(musics));
        }
        return playlistList;
    }

    public PlaylistPersonalDto GetDtoFromPlaylistPersonal(PlaylistPersonal playlist)
    {
        var playlistDto = new PlaylistPersonalDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(playlist.Musics),
            CustumerId = playlist.Customer?.Id ?? Guid.Empty
        };

        return playlistDto;
    }

    public List<PlaylistPersonalDto> GetDtoListFromPlaylistPersonalList(List<PlaylistPersonal> playlists)
    {
        var playlistDtoList = new List<PlaylistPersonalDto>();

        foreach (var playlist in playlists)
        {
            var playlistDto = GetDtoFromPlaylistPersonal(playlist);
            playlistDtoList.Add(playlistDto);
        }

        return playlistDtoList;
    }
}
