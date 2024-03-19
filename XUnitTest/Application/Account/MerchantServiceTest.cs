using Application.Account.Dto;
using Application.Transactions.Dto;
using AutoMapper;
using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Core.Interfaces;
using Domain.Streaming.Agreggates;
using Domain.Transactions.Agreggates;
using Moq;
using Repository;
using System.Linq.Expressions;

namespace Application.Account;
public class MerchantServiceTest
{
    private Mock<IMapper> mapperMock;
    private readonly Mock<ICrypto> cryptoMock;
    private Mock<IRepository<Merchant>> merchantRepositoryMock;
    private Mock<IRepository<Flat>> flatRepositoryMock;
    private readonly MerchantService merchantService;    
    private readonly List<Merchant> mockListMerchant = MockMerchant.Instance.GetListFaker(5);
    public MerchantServiceTest()
    {        

        mapperMock = new Mock<IMapper>();
        cryptoMock = new Mock<ICrypto>();
        merchantRepositoryMock = Usings.MockRepositorio(mockListMerchant);
        flatRepositoryMock = Usings.MockRepositorio(new List<Flat>());
        
        merchantService = new MerchantService(
            mapperMock.Object,
            merchantRepositoryMock.Object,
            flatRepositoryMock.Object
        );
    }

    [Fact]
    public void Create_Merchant_Successfully()
    {
        // Arrange
        var mockMerchant = MockMerchant.Instance.GetFaker();
        var mockCard = MockCard.Instance.GetFaker();
        var mockFlat = MockFlat.Instance.GetFaker();
        var mockMerchantDto = MockMerchant.Instance.GetDtoFromMerchant(mockMerchant);
        mockMerchantDto.FlatId = mockFlat.Id;
        mockMerchantDto.Card = new CardDto()
        {
            Limit = 1000,
            Number = mockCard.Number,
            Validate = mockCard.Validate.Value,
            CVV = mockCard.CVV
        };

        flatRepositoryMock.Setup(repo => repo.GetById(mockFlat.Id)).Returns(mockFlat);
        mapperMock.Setup(mapper => mapper.Map<Card>(mockMerchantDto.Card)).Returns(mockCard);
        mapperMock.Setup(mapper => mapper.Map<Address>(mockMerchantDto.Address)).Returns(mockMerchant.Addresses.Last());
        mapperMock.Setup(mapper => mapper.Map<Merchant>(mockMerchantDto)).Returns(mockMerchant);
        mapperMock.Setup(mapper => mapper.Map<MerchantDto>(It.IsAny<Merchant>())).Returns(mockMerchantDto);
        

        // Act
        var result = merchantService.Create(mockMerchantDto);

        // Assert
        merchantRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Merchant, bool>>>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.GetById(mockFlat.Id), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Card>(mockMerchantDto.Card), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<Address>(mockMerchantDto.Address), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<MerchantDto>(It.IsAny<Merchant>()), Times.Once);
        merchantRepositoryMock.Verify(repo => repo.Save(It.IsAny<Merchant>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockMerchantDto.Name, result.Name);
        Assert.Equal(mockMerchantDto.Email, result.Email);
    }

    [Fact]
    public void Create_Merchant_With_Existing_Email_Fails()
    {
        // Arrange
        var merchantDto = new MerchantDto {  Email = "existing.email@example.com" };

        merchantRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Merchant, bool>>>())).Returns(true);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => merchantService.Create(merchantDto));
        Assert.Equal("Usuário já existente na base.", exception.Message);
        merchantRepositoryMock.Verify(repo => repo.Exists(It.IsAny<Expression<Func<Merchant, bool>>>()), Times.Once);
        flatRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Create_Merchant_With_Nonexistent_Flat_Fails()
    {
        // Arrange
        var merchantDto = new MerchantDto();

        flatRepositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns((Flat)null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => merchantService.Create(merchantDto));
        Assert.Equal("Plano não existente ou não encontrado.", exception.Message);

        flatRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Once);
        merchantRepositoryMock.Verify(repo => repo.Save(It.IsAny<Merchant>()), Times.Never);
    }

    [Fact]
    public void FindAll_Merchants_Successfully()
    {
        // Arrange
        var merchantDtos = MockMerchant.Instance.GetDtoListFromMerchantList(mockListMerchant);
        var userId = mockListMerchant.First().Id;
        mapperMock.Setup(mapper => mapper.Map<List<MerchantDto>>(It.IsAny<List<Merchant>>())).Returns(merchantDtos.FindAll(c => c.Id.Equals(userId)));
        
        // Act
        var result = merchantService.FindAll(userId);

        // Assert
        merchantRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<List<MerchantDto>>(It.IsAny<List<Merchant>>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(mockListMerchant.FindAll(c => c.Id.Equals(userId)).Count, result.Count);
        Assert.All(result, merchantDto => Assert.Equal(userId, merchantDto.Id));
    }

    [Fact]
    public void FindById_Merchant_Successfully()
    {
        // Arrange
        var mockMerchants =  MockMerchant.Instance.GetListFaker(3);

        var mockMerchant = MockMerchant.Instance.GetFaker();
        var merchantId = mockMerchant.Id;
        mockMerchants.Add(mockMerchant);
        mockMerchant.Id = merchantId;
        var merchantDto = new MerchantDto()
        {
            Name = mockMerchant.Name,
            Email = mockMerchant.User.Login.Email,
            Password = mockMerchant.User.Login.Password,
            CPF = mockMerchant.Customer.CPF,
            CNPJ = mockMerchant.CNPJ,
            Phone = mockMerchant.Customer.Phone.Number,
            Address = new AddressDto
            {
                Zipcode = mockMerchant.Addresses.Last().Zipcode,
                Street = mockMerchant.Addresses.Last().Street,
                Number = mockMerchant.Addresses.Last().Number,
                Neighborhood = mockMerchant.Addresses.Last().Neighborhood,
                City = mockMerchant.Addresses.Last().City,
                State = mockMerchant.Addresses.Last().State,
                Complement = mockMerchant.Addresses.Last().Complement,
                Country = mockMerchant.Addresses.Last().Country
            }
        };

        merchantRepositoryMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(mockMerchant);
        mapperMock.Setup(mapper => mapper.Map<MerchantDto>(It.IsAny<Merchant>())).Returns(merchantDto);

        // Act
        var result = merchantService.FindById(merchantId);

        // Assert
        merchantRepositoryMock.Verify(repo => repo.GetById(merchantId), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<MerchantDto>(mockMerchant), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockMerchant.Name, result.Name);
    }

    [Fact]
    public void Update_Merchant_Successfully()
    {
        // Arrange
        var mockMerchant = MockMerchant.Instance.GetFaker();
        var merchantDto = new MerchantDto()
        {
            Name = mockMerchant.Name,
            Email = mockMerchant.User.Login.Email,
            Password = mockMerchant.User.Login.Password,
            CPF = mockMerchant.Customer.CPF,
            CNPJ = mockMerchant.CNPJ,
            Phone = mockMerchant.Customer.Phone.Number,
            Address = new AddressDto
            {
                Zipcode = mockMerchant.Addresses.Last().Zipcode,
                Street = mockMerchant.Addresses.Last().Street,
                Number = mockMerchant.Addresses.Last().Number,
                Neighborhood = mockMerchant.Addresses.Last().Neighborhood,
                City = mockMerchant.Addresses.Last().City,
                State = mockMerchant.Addresses.Last().State,
                Complement = mockMerchant.Addresses.Last().Complement,
                Country = mockMerchant.Addresses.Last().Country
            }
        };

        mapperMock.Setup(mapper => mapper.Map<Merchant>(merchantDto)).Returns(mockMerchant);
        merchantRepositoryMock.Setup(repo => repo.Update(mockMerchant));
        mapperMock.Setup(mapper => mapper.Map<MerchantDto>(mockMerchant)).Returns(merchantDto);

        // Act
        var result = merchantService.Update(merchantDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Merchant>(merchantDto), Times.Once);
        merchantRepositoryMock.Verify(repo => repo.Update(mockMerchant), Times.Once);
        mapperMock.Verify(mapper => mapper.Map<MerchantDto>(mockMerchant), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(merchantDto.Name, result.Name);
    }

    [Fact]
    public void Delete_Merchant_Successfully()
    {
        // Arrange
        var mockMerchant = MockMerchant.Instance.GetFaker();
        var merchantDto = new MerchantDto()
        {
            Name = mockMerchant.Name,
            Email = mockMerchant.User.Login.Email,
            Password = mockMerchant.User.Login.Password,
            CPF = mockMerchant.Customer.CPF,
            CNPJ = mockMerchant.CNPJ,
            Phone = mockMerchant.Customer.Phone.Number,
            Address = new AddressDto
            {
                Zipcode = mockMerchant.Addresses.Last().Zipcode,
                Street = mockMerchant.Addresses.Last().Street,
                Number = mockMerchant.Addresses.Last().Number,
                Neighborhood = mockMerchant.Addresses.Last().Neighborhood,
                City = mockMerchant.Addresses.Last().City,
                State = mockMerchant.Addresses.Last().State,
                Complement = mockMerchant.Addresses.Last().Complement,
                Country = mockMerchant.Addresses.Last().Country
            }
        };

        mapperMock.Setup(mapper => mapper.Map<Merchant>(merchantDto)).Returns(mockMerchant);
        merchantRepositoryMock.Setup(repo => repo.Delete(mockMerchant));

        // Act
        var result = merchantService.Delete(merchantDto);

        // Assert
        mapperMock.Verify(mapper => mapper.Map<Merchant>(merchantDto), Times.Once);
        merchantRepositoryMock.Verify(repo => repo.Delete(mockMerchant), Times.Once);

        Assert.True(result);
    }
}