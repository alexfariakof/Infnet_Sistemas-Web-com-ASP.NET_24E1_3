global using Xunit;
global using __mock__;
using Microsoft.EntityFrameworkCore;
using Moq;
using Domain.Transactions.ValueObject;
using Domain.Core.Aggreggates;
using System.Linq.Expressions;
using Application.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Account.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository.Persistency.Abstractions.Interfaces;

public static class Usings
{
    public static Mock<DbSet<T>> MockDbSet<T>(List<T> data, DbContext? context = null)
    where T : class
    {
        var queryableData = data.AsQueryable();
        var dbSetMock = new Mock<DbSet<T>>();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        dbSetMock
            .As<IQueryable<T>>()
            .Setup(m => m.GetEnumerator())
            .Returns(() => queryableData.GetEnumerator());
        dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);
        dbSetMock
            .Setup(d => d.Update(It.IsAny<T>()))
            .Callback<T>(item =>
            { // Atualizar a entidade no contexto
                var existingItem = data.FirstOrDefault(i => i == item);
                if (existingItem != null)
                {
                    context.Attach(item);
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChangesAsync().Wait();
                }
            });
        dbSetMock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(item => data.Remove(item));
        return dbSetMock;
    }

    public static Mock<IRepository<T>> MockRepositorio<T>(List<T> _dataSet) where T : Base, new()
    {
        var _mock = new Mock<IRepository<T>>();

        _mock.Setup(repo => repo.Save(It.IsAny<T>()));
        _mock.Setup(repo => repo.Update(It.IsAny<T>()));
        _mock.Setup(repo => repo.Delete(It.IsAny<T>()));

        _mock.Setup(repo => repo.FindAll())
            .Returns(() => { return _dataSet.AsEnumerable(); });

        _mock.Setup(repo => repo.GetById(It.IsAny<Guid>()))
            .Returns(
            (Guid id) =>
            {
                return _dataSet.SingleOrDefault(item => item.Id == id);
            });


        _mock.Setup(repo => repo.Find(It.IsAny<Expression<Func<T, bool>>>()))
            .Returns(
            (Expression<Func<T, bool>> expression) =>
            {
                return _dataSet.Where(expression.Compile());
            });

        _mock.Setup(repo => repo.Exists(It.IsAny<Expression<Func<T, bool>>>()));
        return _mock;
    }

    public static Mock<IRepository<CreditCardBrand>> MockDataSetCreditCardBrand()
    {
        var _mock = new Mock<IRepository<CreditCardBrand>>();

        _mock.Setup(repo => repo.GetById(It.IsAny<int>()))
            .Returns(
            (int id) =>
            {
                var creditCardBrandData = new List<CreditCardBrand>
                {
                    new CreditCardBrand((int)CardBrand.Invalid, CardBrand.Invalid.ToString()),
                    new CreditCardBrand((int)CardBrand.Visa, CardBrand.Visa.ToString()),
                    new CreditCardBrand((int)CardBrand.Mastercard, CardBrand.Mastercard.ToString()),
                    new CreditCardBrand((int)CardBrand.Amex, CardBrand.Amex.ToString()),
                    new CreditCardBrand((int)CardBrand.Discover, CardBrand.Discover.ToString()),
                    new CreditCardBrand((int)CardBrand.DinersClub, CardBrand.DinersClub.ToString()),
                    new CreditCardBrand((int)CardBrand.JCB, CardBrand.JCB.ToString())
                };

                return creditCardBrandData.SingleOrDefault(item => item.Id == id);
            });
        

        return _mock;
    }

    public static Mock<IRepository<PerfilUser>> MockDataSetUserType()
    {
        var _mock = new Mock<IRepository<PerfilUser>>();

        _mock.Setup(repo => repo.GetById(It.IsAny<int>()))
            .Returns(
            (int id) =>
            {
                var userTypeData = new List<PerfilUser>
                {
                    new PerfilUser(PerfilUser.UserType.Admin),
                    new PerfilUser(PerfilUser.UserType.Customer),
                    new PerfilUser(PerfilUser.UserType.Merchant)
                };

                return userTypeData.SingleOrDefault(item => item.Id == id);
            });
        return _mock;
    }

    public static void SetupBearerToken(Guid userId, ControllerBase controller, PerfilUser.UserType userType = PerfilUser.UserType.Customer)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("UserType", new PerfilUser(userType).Description),
            new Claim("role", userType.ToString()),
        };
        var identity = new ClaimsIdentity(claims, "UserId");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContext.Request.Headers.Authorization = "Bearer " + Usings.GenerateJwtToken(userId, userType);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    public static string GenerateJwtToken(Guid userId, PerfilUser.UserType userType = PerfilUser.UserType.Customer)
    {
        var options = Options.Create(new TokenOptions
        {
            Issuer = "testIssuer",
            Audience = "testAudience",
            Seconds = 3600,
            DaysToExpiry = 1
        });

        var signingConfigurations = new SigningConfigurations(options);
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(signingConfigurations.Key.ToString())
        );
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] { 
            new Claim("UserId", userId.ToString()),
            new Claim("role", userType.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: signingConfigurations.TokenConfiguration.Issuer,
            audience: signingConfigurations.TokenConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(signingConfigurations.TokenConfiguration.Seconds),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public static DefaultHttpContext MockHttpContext()
    {
        Mock<ISession> sessionMock;
        Dictionary<string, object> sessionStorage;
        sessionMock = new Mock<ISession>();
        sessionStorage = new Dictionary<string, object>();
        sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>())).Callback<string, byte[]>((key, value) => sessionStorage[key] = value);
        sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                if (sessionStorage.TryGetValue(key, out var storedValue))
                {
                    value = (byte[])storedValue;
                    return true;
                }
                byte[]? nullByte = null;
                value = nullByte;
                return false;
            });

        var httpContext = new DefaultHttpContext();
        httpContext.Session = sessionMock.Object;
        return httpContext;
    }
}