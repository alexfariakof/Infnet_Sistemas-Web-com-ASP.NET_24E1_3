using AutoMapper;
using Application.Account.Profile;
using Application.Account.Dto;
using Domain.Streaming.Agreggates;

namespace Application.Account;
public class BandProfileTest
{
    [Fact]
    public void Map_BandDto_To_Band_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BandProfile>();
        }));

        var mockband = MockBand.Instance.GetFaker();
        var bandDto = MockBand.Instance.GetDtoFromBand(mockband);

        // Act
        var band = mapper.Map<Band>(bandDto);

        // Assert
        Assert.NotNull(band);
        Assert.Equal(bandDto.Id, band.Id);
        Assert.Equal(bandDto.Name, band.Name);
        Assert.Equal(bandDto.Description, band.Description);
        Assert.Equal(bandDto.Backdrop, band.Backdrop);
        Assert.NotNull(band.Albums);
        Assert.NotEmpty(band.Albums);
    }

    [Fact]
    public void Map_Band_To_BandDto_IsValid()
    {
        // Arrange
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BandProfile>();
        }));

        var band = MockBand.Instance.GetFaker();
        band.AddAlbum(MockAlbum.Instance.GetFaker());

        // Act
        var bandDto = mapper.Map<BandDto>(band);

        // Assert
        Assert.NotNull(bandDto);
        Assert.Equal(band.Id, bandDto.Id);
        Assert.Equal(band.Name, bandDto.Name);
        Assert.Equal(band.Description, bandDto.Description);
        Assert.Equal(band.Backdrop, bandDto.Backdrop);
        Assert.NotNull(bandDto.Album);
        Assert.Equal(band.Albums.First().Id, bandDto.Album.Id);
    }
}
