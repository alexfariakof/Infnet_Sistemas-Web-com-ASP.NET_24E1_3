using Domain.Account.ValueObject;

namespace Domain.Account;
public class LoginTest
{
    [Fact]
    public void Should_Success_With_Valid_Login()
    {
        // Arrange
        var mockLogin = MockLogin.Instance.GetFaker();

        // Act
        var login = new Login() 
        { 
            Email = mockLogin.Email,
            Password = mockLogin.Password,
        };

        // Assert
        Assert.Equal(mockLogin.Email, login.Email);
        Assert.NotEqual(mockLogin.Password, login.Password);
    }

    [Fact]
    public void Should_Throws_Erro_With_Invalid_Email()
    {
        // Arrange & Act 
        var mockLogin = new Login();
        var exception = () => mockLogin.Email = "InvalidEmail";
        
        // Assert
        Assert.Throws<ArgumentException>(exception);
    }
            
    [Fact]
    public void Should_Throws_Erro_With_Long_Email()
    {
        // Arrange & Act 
        var mockLogin = MockLogin.Instance.GetFaker();

        var exception = () => mockLogin.Email = new string('a', 257);
        
        // Assert
        Assert.Throws<ArgumentException>(exception);
    }
}
