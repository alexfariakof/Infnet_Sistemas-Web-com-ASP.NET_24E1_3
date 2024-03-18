﻿using Bogus;
using Domain.Streaming.Agreggates;

namespace __mock__;
public class MockGenre
{
    private static readonly Lazy<MockGenre> _instance = new Lazy<MockGenre>(() => new MockGenre());

    public static MockGenre Instance => _instance.Value;

    private MockGenre() { }

    public Genre GetFaker()
    {
        var fakeGenre = new Faker<Genre>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(a => a.Name, f => f.Address.ZipCode())
            .Generate();

        return fakeGenre;
    }

    public List<Genre> GetListFaker(int count)
    {
        var addressList = new List<Genre>();
        for (var i = 0; i < count; i++)
        {
            addressList.Add(GetFaker());
        }
        return addressList;
    }
    
}
