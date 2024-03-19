using Domain.Streaming.Agreggates;
using Domain.Streaming.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Streaming;
public class MusicMap : IEntityTypeConfiguration<Music>
{
    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.ToTable("Music");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Url).IsRequired();

        builder.OwnsOne<Duration>(d => d.Duration, c =>
        {
            c.Property(x => x.Value).HasColumnName("Duration").IsRequired().HasMaxLength(50);
        });

        builder.HasOne(x => x.Album).WithMany(m => m.Musics);
        builder.HasMany(x => x.Playlists).WithMany(m => m.Musics);
        builder.HasMany(x => x.Genres).WithMany(m => m.Musics);

        builder.HasMany(x => x.Flats)
            .WithMany(x => x.Musics)
            .UsingEntity<Dictionary<string, object>>(
            "FlatMusic",
            j => j
            .HasOne<Flat>()
            .WithMany(),
            j => j
            .HasOne<Music>()
            .WithMany(),
            j =>
            {
                j.Property<DateTime>("DtAdded").HasDefaultValue(DateTime.Now);
            });

    }
}