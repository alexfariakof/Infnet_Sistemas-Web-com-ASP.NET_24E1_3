using Bogus;
using Domain.Account.ValueObject;

namespace __mock__;
public class MockLogin
{
    private static readonly Lazy<MockLogin> instance = new Lazy<MockLogin>(() => new MockLogin());

    public static MockLogin Instance => instance.Value;

    private MockLogin() { }

    public Login GetFaker()
    {
        var fakeLogin = new Faker<Login>()
            .RuleFor(l => l.Email, f => f.Internet.Email())
            .RuleFor(l => l.Password, f => f.Internet.Password())
            .Generate();

        return fakeLogin;
    }
}
