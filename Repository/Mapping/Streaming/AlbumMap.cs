using Domain.Streaming.Agreggates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Mapping.Streaming
{
    public class AlbumMap : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable(nameof(Album));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Backdrop).IsRequired();

            builder.HasMany(x => x.Musics).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Genres).WithMany(m => m.Albums);

            builder.HasMany(x => x.Flats)
                .WithMany(x => x.Albums)
                .UsingEntity<Dictionary<string, object>>(
                "FlatAlbum",
                j => j
                .HasOne<Flat>()
                .WithMany()
                .HasForeignKey("FlatId"),
                j => j
                .HasOne<Album>()
                .WithMany()
                .HasForeignKey("AlbumId"),
                j =>
                {
                    j.HasKey("FlatId", "AlbumId");
                    j.Property<DateTime>("DtAdded").HasDefaultValue(DateTime.Now);
                });
        }
    }
 }
