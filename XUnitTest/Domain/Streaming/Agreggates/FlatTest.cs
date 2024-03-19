using Domain.Streaming.Agreggates;
using __mock__;

namespace Domain.Streaming;
public class FlatTest
{
    [Fact]
    public void Should_Set_Properties_Correctly_Flat()
    {
        // Arrange
        var expectedFlat = MockFlat.Instance.GetFaker();
        var actualFlat = new Flat
        {
            Id = expectedFlat.Id,
            Name = expectedFlat.Name,
            Description = expectedFlat.Description,
            Value = expectedFlat.Value
        };

        // Assert
        Assert.NotNull(actualFlat);
        Assert.Equal(expectedFlat.Id, actualFlat.Id);
        Assert.Equal(expectedFlat.Name, actualFlat.Name);
        Assert.Equal(expectedFlat.Description, actualFlat.Description);
        Assert.Equal(expectedFlat.Value, actualFlat.Value);
    }
}