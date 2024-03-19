using Domain.Streaming.Agreggates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Mapping.Streaming
{ 
    public class BandMap : IEntityTypeConfiguration<Band>
    {
        public void Configure(EntityTypeBuilder<Band> builder)
        {
            builder.ToTable(nameof(Band));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Backdrop).IsRequired();
            builder.HasMany<Album>(x => x.Albums).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Genres).WithMany(m => m.Bands);
        }
    }
}