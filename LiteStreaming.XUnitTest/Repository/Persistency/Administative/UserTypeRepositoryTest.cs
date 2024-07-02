using Domain.Administrative.ValueObject;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Repository.Persistency.Administrative;
public class PerfilRepositoryTest
{
    private Mock<RegisterContextAdmin> contextMock;

    public PerfilRepositoryTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContextAdmin>().UseInMemoryDatabase(databaseName: "TestDatabase_PerfilRepositoryTest").Options;
        contextMock = new Mock<RegisterContextAdmin>(options);
    }

    [Fact]
    public void GetById_Should_Return_UserType_With_Correct_Id()
    {
        // Arrange
        contextMock.Object.Add(Usings.MockDataSetUserType().Object);
        var repository = new PerfilRepository(contextMock.Object);
        var mockPerfil = new Perfil(Perfil.UserType.Normal);
        contextMock.Setup<Perfil>(c => c.Set<Perfil>().Find(It.IsAny<int>())).Returns((Perfil)mockPerfil);        

        // Act
        var result = repository.GetById(mockPerfil.Id);

        // Assert
        Assert.Equal(mockPerfil, result);
    }
}