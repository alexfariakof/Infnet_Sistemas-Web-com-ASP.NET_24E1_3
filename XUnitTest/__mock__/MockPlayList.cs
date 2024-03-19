using Application.Account.Dto;
using Bogus;
using Domain.Streaming.Agreggates;

namespace __mock__;
public sealed class MockPlaylist
{
    private static readonly Lazy<MockPlaylist> _instance = new Lazy<MockPlaylist>(() => new MockPlaylist());

    private MockPlaylist() { }

    public static MockPlaylist Instance => _instance.Value;

    public Playlist GetFaker()
    {
        var fakePlaylist = new Faker<Playlist>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Lorem.Word())
            .RuleFor(p => p.Flats, f => new List<Flat>())
            .RuleFor(p => p.Musics, f => new List<Music>())
            .Generate();

        return fakePlaylist;
    }

    public List<Playlist> GetListFaker(int count)
    {
        var playlistList = new List<Playlist>();
        for (var i = 0; i < count; i++)
        {
            playlistList.Add(GetFaker());
        }
        return playlistList;
    }

    public PlaylistDto GetDtoFromPlaylist(Playlist playlist)
    {
        var playlistDto = new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Musics = MockMusic.Instance.GetDtoListFromMusicList(playlist.Musics)
        };

        return playlistDto;
    }

    public List<PlaylistDto> GetDtoListFromPlaylistList(List<Playlist> playlists)
    {
        var playlistDtoList = new List<PlaylistDto>();
        foreach (var playlist in playlists)
        {
            var playlistDto = GetDtoFromPlaylist(playlist);
            playlistDtoList.Add(playlistDto);
        }

        return playlistDtoList;
    }
}
