using Domain.Streaming.Agreggates;
using Domain.Streaming.ValueObject;
using Repository.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Streaming;
public class MusicMap : IEntityTypeConfiguration<Music>
{
    private readonly DefaultValueSqlConstants baseConstants;
    public MusicMap(DefaultValueSqlConstants baseConstants) : base()
    {
        this.baseConstants = baseConstants;
    }

    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.ToTable("Music");
        builder.Property(music => music.Id).HasColumnType("binary(16)")
        .HasConversion(
            v => v.ToByteArray(),
            v => new Guid(v)
        )
        .ValueGeneratedOnAdd();
        builder.HasKey(music => music.Id);
        builder.Property(music => music.Name).IsRequired().HasMaxLength(50);
        builder.Property(music => music.Url).IsRequired();

        builder.OwnsOne<Duration>(d => d.Duration, c =>
        {
            c.Property(music => music.Value).HasColumnName("Duration").IsRequired().HasMaxLength(50);
        });

        builder.HasOne(music => music.Band).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(music => music.Album).WithMany(m => m.Musics).OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(music => music.Playlists).WithMany(m => m.Musics);
        builder.HasMany(music => music.Genres).WithMany(m => m.Musics);

        builder.HasMany(music => music.Flats).WithMany(music => music.Musics).UsingEntity<Dictionary<string, object>>("FlatMusic",
            dictonary => dictonary.HasOne<Flat>().WithMany(),
            dictonary => dictonary.HasOne<Music>().WithMany(),
            dictonary =>
            {
                dictonary.Property<DateTime?>("DtAdded").ValueGeneratedOnAdd().HasDefaultValueSql(baseConstants.CURRENT_DATE);
            });

    }
}