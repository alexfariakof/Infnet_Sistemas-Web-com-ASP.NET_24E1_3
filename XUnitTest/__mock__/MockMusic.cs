using Application.Account.Dto;
using Bogus;
using Domain.Streaming.Agreggates;
using Domain.Streaming.ValueObject;

namespace __mock__;
public class MockMusic
{
    private static readonly Lazy<MockMusic> _instance = new Lazy<MockMusic>(() => new MockMusic());

    private MockMusic() { }

    public static MockMusic Instance => _instance.Value;

    public Music GetFaker()
    {
        var fakeMusic = new Faker<Music>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Commerce.ProductName())
            .RuleFor(m => m.Duration, f => new Duration(f.Random.Int(60, 600)))
            .RuleFor(m => m.Album, new Album())
            .RuleFor(m => m.Playlists, new List<Playlist>())
            .Generate();
        
        return fakeMusic;
    }

    public List<Music> GetListFaker(int count)
    {
        var mockPlaylist = MockPlaylist.Instance.GetFaker();
        var musicList = new List<Music>();
        for (var i = 0; i < count; i++)
        {
            var mockMusic = GetFaker();
            mockPlaylist.Musics.Add(mockMusic);
            mockMusic.Playlists.Add(mockPlaylist);
            musicList.Add(mockMusic);
        }
        return musicList;
    }

    public MusicDto GetDtoFromMusic(Music music)
    {
        var musicDto = new MusicDto
        {
            Id = music.Id,
            Name = music.Name,
            Duration = music.Duration.Value,
            FlatId = Guid.NewGuid()
        };

        return musicDto;
    }

    public List<MusicDto> GetDtoListFromMusicList(IList<Music> musics)
    {
        var musicDtoList = new List<MusicDto>();
        foreach (var music in musics)
        {
            var musicDto = GetDtoFromMusic(music);
            musicDtoList.Add(musicDto);
        }
        return musicDtoList;
    }
}
