using Domain.Streaming.Agreggates;
using Repository;

namespace DataSeeders.Implementations;
public class DataSeederFlat : IDataSeeder
{
    private readonly RegisterContext _context;
    public DataSeederFlat(RegisterContext context)
    {
        _context = context;
    }
    public void SeedData()
    {
        _context.Flat.AddRange(
            new Flat
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5"),
                Name = "Free Flat",
                Description = "Free Flat Desciption",
                Value = 250m
            },

            new Flat
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Name = "Basic Flat",
                Description = "Basic Flat Desciption",
                Value = 250m
            },
            new Flat
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                Name = "Standard  Flat",
                Description = "Standard  Flat Desciption",
                Value = 500m
            },
            new Flat
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa8"),
                Name = "Premium  Flat",
                Description = "Premium  Flat Desciption",
                Value = 750m
            });
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Flat.RemoveRange();
            Console.WriteLine($"Dados já cadastrados na base de dados");
        }
    }
}