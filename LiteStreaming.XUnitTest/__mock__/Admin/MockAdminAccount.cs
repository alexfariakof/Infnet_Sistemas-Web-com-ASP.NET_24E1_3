using Bogus;
using Application.Administrative.Dto;
using Domain.Administrative.Agreggates;
using Domain.Administrative.ValueObject;

namespace __mock__.Admin;
public class MockAdminAccount
{
    private static MockAdminAccount? _instance;
    private static readonly object LockObject = new object();
    public static MockAdminAccount Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new MockAdminAccount();
            }
        }
    }

    private MockAdminAccount() { }

    public AdminAccount GetFaker()
    {
        var fakeAdminAccount = new Faker<AdminAccount>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Login, MockLogin.Instance.GetFaker())
            .RuleFor(a => a.DtCreated, f => f.Date.Past())
            .RuleFor(a => a.Name, f => f.Name.FirstName())
            .RuleFor(a => a.PerfilType, f => new Perfil(Perfil.UserType.Admin));

        return fakeAdminAccount;
    }

    public AdminAccountDto GetFakerDto()
    {
        lock (LockObject)
        {
            var fakeAdminAccountDto = new Faker<AdminAccountDto>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Email, f => f.Internet.Email())
                .RuleFor(a => a.Password, f => f.Internet.Password())
                .RuleFor(a => a.PerfilType, f => (PerfilDto)Perfil.UserType.Admin)
                .Generate();
            fakeAdminAccountDto.UsuarioId = fakeAdminAccountDto.Id;
            return fakeAdminAccountDto;
        }
    }

    public List<AdminAccount> GetListFaker(int count = 2)
    {
        lock (LockObject)
        {
            var AdminAccounts = new List<AdminAccount>();

            for (int i = 0; i < count; i++)
            {
                var account = GetFaker();
                account.PerfilType = (i + 2) % 2 == 0 ? Perfil.UserType.Admin : Perfil.UserType.Normal;
                AdminAccounts.Add(account);
            }

            return AdminAccounts;
        }
    }

    public AdminAccountDto GetFakerDto(AdminAccount? account = null)
    {
        lock (LockObject)
        {
            if (account == null) account = MockAdminAccount.Instance.GetFaker();

            var fakeAdminAccountDto = new Faker<AdminAccountDto>()
                .RuleFor(a => a.Id, f => account.Id)
                .RuleFor(a => a.UsuarioId, f => account.Id)
                .RuleFor(a => a.Name, f => account.Name)
                .RuleFor(a => a.Email, f => account.Login.Email)
                .RuleFor(a => a.Password, f => f.Internet.Password())
                .RuleFor(a => a.PerfilType, f => (PerfilDto)account.PerfilType.Id)
                .Generate();

            return fakeAdminAccountDto;
        }
    }

    public List<AdminAccountDto> GetFakerListDto(IList<AdminAccount>? accounts = null, int count = 3)
    {
        lock (LockObject)
        {
            if (accounts == null) accounts = MockAdminAccount.Instance.GetListFaker(count);

            var AdminAccountDtoList = new List<AdminAccountDto>();

            foreach (var account in accounts)
            {
                var accountDto = GetFakerDto(account);
                AdminAccountDtoList.Add(accountDto);
            }

            return AdminAccountDtoList;
        }
    }
}