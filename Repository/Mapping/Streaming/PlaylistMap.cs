using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Streaming.Agreggates;

namespace Repository.Mapping.Streaming
{
    public class PlaylistMap : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.ToTable(nameof(Playlist));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Backdrop).IsRequired();
            
            builder.HasMany(x => x.Genres).WithMany(x => x.Playlists);

            builder.HasMany(x => x.Musics)
                    .WithMany(x => x.Playlists)
                    .UsingEntity<Dictionary<string, object>>(
                    "MusicPlayList",
                    j => j
                        .HasOne<Music>()
                        .WithMany(),
                    j => j
                        .HasOne<Playlist>()
                        .WithMany(),
                    j =>
                    {
                        j.Property<DateTime>("DtAdded").HasDefaultValue(DateTime.Now);
                    });

            builder.HasMany(x => x.Flats)
                .WithMany(x => x.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                "FlatPlayList",
                j => j
                .HasOne<Flat>()
                .WithMany(),
                j => j
                .HasOne<Playlist>()
                .WithMany(),
                j =>
                {
                    j.Property<DateTime>("DtAdded").HasDefaultValue(DateTime.Now);
                });
        }
    }
}