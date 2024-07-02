using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Account.Agreggates;
using Domain.Streaming.Agreggates;
using Repository.Constants;

namespace Repository.Mapping.Account;
public class PlaylistPersonalMap : IEntityTypeConfiguration<PlaylistPersonal>
{
    private readonly DefaultValueSqlConstants baseConstants;
    public PlaylistPersonalMap(DefaultValueSqlConstants baseConstants) : base()
    {
        this.baseConstants = baseConstants;
    }

    public void Configure(EntityTypeBuilder<PlaylistPersonal> builder)
    {
        builder.ToTable(nameof(PlaylistPersonal));

        builder.HasKey(playlist => playlist.Id);
        builder.Property(playlist => playlist.Id).ValueGeneratedOnAdd();
        builder.Property(playlist => playlist.Name).IsRequired().HasMaxLength(50);
        builder.Property(playlist => playlist.CustomerId).IsRequired();
        builder.Property(playlist => playlist.IsPublic).IsRequired();
        builder.Property(playlist => playlist.DtCreated).ValueGeneratedOnAdd();

        builder.HasMany(playlist => playlist.Musics)
        .WithMany(music => music.PersonalPlaylists).UsingEntity<Dictionary<string, object>>("MusicPlayListPersonal",
            dictonary => dictonary.HasOne<Music>().WithMany().HasForeignKey("MusicId"),
            dictonary => dictonary.HasOne<PlaylistPersonal>().WithMany().HasForeignKey("PlaylistPersonalId"),
            dictonary =>
            {
                dictonary.HasKey("MusicId", "PlaylistPersonalId");
                dictonary.Property<DateTime>("DtAdded").ValueGeneratedOnAdd().HasDefaultValueSql(baseConstants.CURRENT_DATE);
            });
    }
}