using __mock__.Admin;

namespace Domain.Administrative;

public class AdministrativeAccountTest
{
    [Fact]
    public void Should_Create_Administrative_Account_With_Login_And_Name()
    {
        // Arrange
        var administrativeAccountMock = MockAdminAccount.Instance.GetFaker();
        var administrativeAccount = administrativeAccountMock;

        // Act - No need for action in this case

        // Assert
        Assert.NotNull(administrativeAccount?.Id);
        Assert.NotNull(administrativeAccount?.Login);
        Assert.NotNull(administrativeAccount?.DtCreated);
        Assert.NotNull(administrativeAccount?.Name);
        Assert.NotNull(administrativeAccount?.PerfilType);
    }
}
