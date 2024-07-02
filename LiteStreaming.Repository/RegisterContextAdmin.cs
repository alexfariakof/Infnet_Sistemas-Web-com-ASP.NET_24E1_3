using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Repository;
public class RegisterContextAdmin: BaseContextAdmin<RegisterContextAdmin>
{
    public RegisterContextAdmin(DbContextOptions<RegisterContextAdmin> options) : base(options) { }
}