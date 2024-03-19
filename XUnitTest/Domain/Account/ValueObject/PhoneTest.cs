using Domain.Account.ValueObject;

namespace Domain.Account.Tests;
public class PhoneTest
{
    [Fact]
    public void Should_Set_Phone_Number_Correctly()
    {
        // Arrange
        var phoneNumber = "(99)-9456-7890";

        // Act
        var phone = new Phone(phoneNumber);

        // Assert
        Assert.Equal(phoneNumber, phone.Number);
    }

    [Fact]
    public void Implicit_Conversion_Should_Work_Correctly()
    {
        // Arrange
        var phoneNumber = "(21)7-9654-3210";

        // Act
        Phone phone = phoneNumber;
        string convertedNumber = phone;

        // Assert
        Assert.Equal(phoneNumber, convertedNumber);
    }

    [Fact]
    public void Should_Throw_Exception_With_Empty_Phone_Number()
    {
        // Arrange & Act
        var exception = Assert.Throws<ArgumentException>(() => new Phone(string.Empty));

        // Assert
        Assert.Equal("Valor do telefone não pode ser em branco", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_With_Null_Phone_Number()
    {
        // Arrange & Act
        var exception = Assert.Throws<ArgumentException>(() => new Phone(null));

        // Assert
        Assert.Equal("Valor do telefone não pode ser em branco", exception.Message);
    }
}
