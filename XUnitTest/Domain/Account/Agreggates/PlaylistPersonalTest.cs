using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;

namespace Domain.Account;
public class PlaylistPersonalTests
{
    [Theory]
    [InlineData(true,  "PlayLIst 1")]
    [InlineData(false, "PlayList 2")]
    public void Should_Set_Properties_Correctly_PlaylistPersonal(bool isPublic, string name)
    {
        // Arrange 
        var customer = MockCustomer.Instance.GetFaker();
        var dtCreated = DateTime.Now;
        // Act
        var playlist = new PlaylistPersonal
        {
            Customer = customer,
            IsPublic = isPublic,
            DtCreated = dtCreated,
            Name = name,
            Musics = new List<Music>()
        };

        // Assert
        Assert.Equal(customer, playlist.Customer);
        Assert.Equal(isPublic, playlist.IsPublic);
        Assert.Equal(dtCreated.Date, playlist.DtCreated.Date);
        Assert.Equal(name, playlist.Name);
        Assert.Empty(playlist.Musics);
    }
}