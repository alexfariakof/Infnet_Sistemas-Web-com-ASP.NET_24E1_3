using Domain.Streaming.Agreggates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Repository.Constants;

namespace Repository.Mapping.Streaming
{
    public class AlbumMap : IEntityTypeConfiguration<Album>
    {
        private readonly DefaultValueSqlConstants baseConstants;
        public AlbumMap(DefaultValueSqlConstants baseConstants) : base()
        {
            this.baseConstants = baseConstants;
        }

        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable(nameof(Album));

            builder.HasKey(album => album.Id);
            builder.Property(album => album.Id).ValueGeneratedOnAdd();
            builder.Property(album => album.Name).IsRequired().HasMaxLength(50);
            builder.Property(album => album.Backdrop).IsRequired();
            builder.HasMany(album => album.Musics).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(album => album.Genres).WithMany(genre => genre.Albums);

            builder.HasMany(album => album.Flats).WithMany(album => album.Albums).UsingEntity<Dictionary<string, object>>("FlatAlbum",
                dictonary => dictonary.HasOne<Flat>().WithMany().HasForeignKey("FlatId"),
                dictonary => dictonary.HasOne<Album>().WithMany().HasForeignKey("AlbumId"),
                dictonary =>
                {
                    dictonary.HasKey("FlatId", "AlbumId");
                    dictonary.Property<DateTime?>("DtAdded").ValueGeneratedOnAdd().HasDefaultValueSql(baseConstants.CURRENT_DATE);
                });
        }
    }
 }
