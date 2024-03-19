using Domain.Account.Agreggates;
using Domain.Account.ValueObject;
using Domain.Transactions.Agreggates;
using Domain.Transactions.ValueObject;
using Repository;

namespace DataSeeders.Implementations
{
    public class DataSeederCustomer : IDataSeeder
    {
        private readonly RegisterContext _context;

        public DataSeederCustomer(RegisterContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            var customer = new Customer
            {
                Name = "Free User Test",
                Birth = new DateTime(1990, 1, 1),
                CPF = "123.456.789-01",
                Phone = new Phone { Number = "+5521993879312" },
                User = new User()
                {
                    Login = new Login { Email = "user@custumer.com", Password = "12345" },
                    UserType = _context.UserType.Where(u => u.Id.Equals(2)).First()
                },
                Flat = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).FirstOrDefault()
            };

            var address = new Address()
            {
                Zipcode = "12345-678",
                Street = "Free Street",
                Number = "123",
                Neighborhood = "Free",
                City = "FreeCityville",
                State = "ST",
                Complement = "Apt 456",
                Country = "FreeCountryland"
            };
            customer.AddAdress(address);

            var card = new Card()
            {
                Number = "5564 7434 7840 3985",
                Validate = new ExpiryDate(new DateTime(2099, 1, 1)),
                CVV = "388",
                CardBrand = _context.CardBrand.Where(c => c.Id == (int)CreditCardBrand.IdentifyCard("5564 7434 7840 3985").CardBrand).FirstOrDefault(),
                Active = true,
                Limit = 1000m
            };
            customer.AddCard(card);
            _context.Add(customer);
            
            customer = new Customer
            {
                Name = "Basic User Test",
                Birth = new DateTime(1992, 1, 1),
                CPF = "123.456.789-02",
                Phone = new Phone { Number = "+5521993879312" },
                User = new User()
                {
                    Login = new Login { Email = "basic@user.com", Password = "12345" },
                    UserType = _context.UserType.Where(u => u.Id.Equals(2)).First()
                },
                Flat = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")).FirstOrDefault()
            };
            address = new Address()
            {
                Zipcode = "12345-678",
                Street = "Basic Street",
                Number = "123",
                Neighborhood = "Basic",
                City = "BasicCityville",
                State = "ST",
                Complement = "Apt 456",
                Country = "BasicCountryland"
            };
            customer.AddAdress(address);
            card = new Card()
            {
                Number = "6011 3044 4018 2277",
                Validate = new ExpiryDate(new DateTime(2024, 9, 4)),
                CVV = "3541",
                CardBrand = _context.CardBrand.Where(c => c.Id == (int)CreditCardBrand.IdentifyCard("6011 3044 4018 2277").CardBrand).FirstOrDefault(),
                Active = true,
                Limit = 1200m
            };
            customer.AddCard(card);
            _context.Add(customer);

            customer = new Customer
            {
                Name = "Standard User Test",
                Birth = new DateTime(1993, 1, 1),
                CPF = "123.456.789-03",
                Phone = new Phone { Number = "+5521993879312" },
                User = new User()
                {
                    Login = new Login { Email = "standard@user.com", Password = "12345" },
                    UserType = _context.UserType.Where(u => u.Id.Equals(2)).First()
                },
                Flat = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7")).FirstOrDefault()
            };
            address = new Address()
            {
                Zipcode = "12345-678",
                Street = "Standard Street",
                Number = "123",
                Neighborhood = "Standard",
                City = "StandardCityville",
                State = "ST",
                Complement = "Apt 456",
                Country = "StandardCountryland"
            };
            customer.AddAdress(address);
            card = new Card()
            {
                Number = "5422 8010 8003 9910",
                Validate = new ExpiryDate(new DateTime(2025, 4, 2)),
                CVV = "549",
                CardBrand = _context.CardBrand.Where(c => c.Id == (int)CreditCardBrand.IdentifyCard("5422 8010 8003 9910").CardBrand).FirstOrDefault(),
                Active = true,
                Limit = 2000m
            };
            customer.AddCard(card);
            _context.Add(customer);

            try
            {                
                _context.SaveChanges();
            }
            catch
            {
                _context.Remove(customer);
                Console.WriteLine($"Dados já cadastrados na base de dados");
            }
        }
    }
}