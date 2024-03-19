using Domain.Account.Agreggates;
using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Account.ValueObject;
using Domain.Transactions.Agreggates;
using Application.Account.Dto;

namespace __mock__;
public class MockCustomer
{
    private static MockCustomer _instance;
    private static readonly object LockObject = new object();
    public static MockCustomer Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new MockCustomer();
            }
        }
    }

    private MockCustomer() { }

    public Customer GetFaker()
    {
        lock (LockObject)
        {
            var fakeCustomer = new Faker<Customer>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.User, MockUser.Instance.GetFaker())
                .RuleFor(c => c.CPF, f => f.Person.Cpf())
                .RuleFor(c => c.Birth, f => f.Person.DateOfBirth)
                .RuleFor(c => c.Phone, f => new Phone { Number = f.Person.Phone })
                .RuleFor(c => c.Addresses, MockAddress.Instance.GetListFaker(1))
                .RuleFor(m => m.Cards, f => new List<Card>())
                .RuleFor(m => m.Signatures, f => new List<Signature>())
                .Generate();

            fakeCustomer.User.UserType = new UserType(UserTypeEnum.Customer);
            return fakeCustomer;
        }
    }

    public List<Customer> GetListFaker(int count)
    {
        lock (LockObject)
        {
            var customerList = new List<Customer>();
            for (var i = 0; i < count; i++)
            {
                customerList.Add(GetFaker());
            }
            return customerList;
        }
    }

    public CustomerDto GetDtoFromCustomer(Customer customer)
    {
        lock (LockObject)
        {
            var fakeCustomerDto = new Faker<CustomerDto>()
                .RuleFor(c => c.Id, f => customer.Id)
                .RuleFor(c => c.Name, f => customer.Name)
                .RuleFor(c => c.Email, f => customer.User.Login.Email)
                .RuleFor(c => c.Password, f => customer.User.Login.Password)
                .RuleFor(c => c.CPF, f => customer.CPF)
                .RuleFor(c => c.Birth, f => customer.Birth)
                .RuleFor(c => c.Phone, f => customer.Phone.Number)
                .RuleFor(c => c.Address, f => MockAddress.Instance.GetDtoFromAddress(customer.Addresses[0]))
                .RuleFor(c => c.FlatId, f => Guid.NewGuid())
                .Generate();

            return fakeCustomerDto;
        }
    }

    public List<CustomerDto> GetDtoListFromCustomerList(IList<Customer> customers)
    {
        lock (LockObject)
        {
            var customerDtoList = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                var customerDto = GetDtoFromCustomer(customer);
                customerDtoList.Add(customerDto);
            }

            return customerDtoList;
        }
    }
}