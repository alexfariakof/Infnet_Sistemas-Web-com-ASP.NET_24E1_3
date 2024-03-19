using Domain.Account.Agreggates;
using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Notifications;
using Application.Account.Dto;
using Domain.Transactions.Agreggates;
using Domain.Account.ValueObject;

namespace __mock__;
public class MockMerchant
{
    private static MockMerchant _instance;
    private static readonly object LockObject = new object();

    public static MockMerchant Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new MockMerchant();
            }
        }
    }

    private readonly Lazy<Faker<Merchant>> fakerInstance;

    private MockMerchant()
    {
        fakerInstance = new Lazy<Faker<Merchant>>(() => new Faker<Merchant>());
    }

    private Faker<Merchant> Faker => fakerInstance.Value;

    public Merchant GetFaker()
    {
        lock (LockObject)
        {
            var mockUser = MockUser.Instance.GetFaker();
            var mockCustomer = MockCustomer.Instance.GetFaker();
            mockCustomer.User = mockUser;
            var fakeMerchant = Faker
                .RuleFor(m => m.Id, f => f.Random.Guid())
                .RuleFor(m => m.Name, f => f.Name.FirstName())
                .RuleFor(c => c.User, mockUser)
                .RuleFor(m => m.Customer, mockCustomer)
                .RuleFor(m => m.CNPJ, f => f.Company.Cnpj())
                .RuleFor(c => c.Addresses, MockAddress.Instance.GetListFaker(1))
                .RuleFor(m => m.Cards, f => new List<Card>())
                .RuleFor(m => m.Signatures, f => new List<Signature>())
                .RuleFor(m => m.Notifications, f => new List<Notification>())
                .Generate();
            fakeMerchant.User.UserType = new UserType(UserTypeEnum.Merchant);
            return fakeMerchant;

        }
    }

    public List<Merchant> GetListFaker(int count)
    {
        lock (LockObject)
        {
            var merchantList = new List<Merchant>();
            for (var i = 0; i < count; i++)
            {
                merchantList.Add(GetFaker());
            }
            return merchantList;
        }
    }

    public MerchantDto GetDtoFromMerchant(Merchant merchant)
    {
        lock (LockObject)
        {
            var fakeMerchantDto = new Faker<MerchantDto>()
                .RuleFor(c => c.Id, f => merchant.Id)
                .RuleFor(c => c.Name, f => merchant.Name)
                .RuleFor(c => c.Email, f => merchant.User.Login.Email)
                .RuleFor(c => c.Password, f => merchant.User.Login.Password)
                .RuleFor(c => c.CPF, f => merchant.Customer.CPF)
                .RuleFor(c => c.CNPJ, f => merchant.CNPJ)
                .RuleFor(c => c.Phone, f => merchant.Customer.Phone.Number)
                .RuleFor(c => c.Address, f => MockAddress.Instance.GetDtoFromAddress(merchant.Addresses[0]))
                .RuleFor(c => c.FlatId, f => Guid.NewGuid())
                .Generate();

            return fakeMerchantDto;
        }
    }

    public List<MerchantDto> GetDtoListFromMerchantList(IList<Merchant> merchants)
    {
        lock (LockObject)
        {
            var merchantDtoList = new List<MerchantDto>();

            foreach (var merchant in merchants)
            {
                var merchantDto = GetDtoFromMerchant(merchant);
                merchantDtoList.Add(merchantDto);
            }

            return merchantDtoList;
        }
    }
}
