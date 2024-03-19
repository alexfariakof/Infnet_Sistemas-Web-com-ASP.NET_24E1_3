using Bogus;
using Domain.Account.Agreggates;

namespace __mock__;
public class MockUser
{
    private static readonly Lazy<MockUser> instance = new Lazy<MockUser>(() => new MockUser());

    public static MockUser Instance => instance.Value;

    private MockUser() { }

    public User GetFaker()
    {
        var fakeLogin = new Faker<User>()
            .RuleFor(l => l.Id, f => Guid.NewGuid())
            .RuleFor(l => l.Login, f => MockLogin.Instance.GetFaker())
            .RuleFor(t => t.DtCreated, f => f.Date.Recent())
            .Generate();

        return fakeLogin;
    }
    public List<User> GetListFaker(int count)
    {
        var userList = new List<User>();
        for (var i = 0; i < count; i++)
        {
            userList.Add(GetFaker());
        }
        return userList;
    }

}
