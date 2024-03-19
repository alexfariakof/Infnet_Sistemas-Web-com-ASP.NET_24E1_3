using Domain.Streaming.Agreggates;
using Repository;

namespace DataSeeders.Implementations;
public class DataSeederGenre : IDataSeeder
{
    private readonly RegisterContext _context;
    public DataSeederGenre(RegisterContext context)
    {
        _context = context;
    }
    public void SeedData()
    {
        _context.Genre.AddRange(
            new Genre
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5"),
                Name = "Rock",
            },

            new Genre
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Name = "Popular",
            },
            new Genre
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                Name = "Pagode",
            },
            new Genre
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                Name = "Funk",
            });
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            Console.WriteLine($"Dados já cadastrados na base de dados");
        }
    }
}