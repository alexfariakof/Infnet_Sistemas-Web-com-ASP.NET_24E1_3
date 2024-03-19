using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations_MySqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddTableandRelationsGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "User",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 10, DateTimeKind.Local).AddTicks(2687),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 0, DateTimeKind.Local).AddTicks(3502));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "PlaylistPersonal",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 4, DateTimeKind.Local).AddTicks(3398),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 23, 990, DateTimeKind.Local).AddTicks(5589));

            migrationBuilder.AddColumn<string>(
                name: "Backdrop",
                table: "Playlist",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayListPersonal",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 8, DateTimeKind.Local).AddTicks(9706),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 23, 997, DateTimeKind.Local).AddTicks(3297));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayList",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 54, DateTimeKind.Local).AddTicks(5161),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 41, DateTimeKind.Local).AddTicks(4891));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Music",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatPlayList",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 61, DateTimeKind.Local).AddTicks(2152),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 46, DateTimeKind.Local).AddTicks(8138));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatMusic",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 45, DateTimeKind.Local).AddTicks(6478),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 33, DateTimeKind.Local).AddTicks(932));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatAlbum",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 29, DateTimeKind.Local).AddTicks(5382),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 14, DateTimeKind.Local).AddTicks(8631));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Band",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Backdrop",
                table: "Band",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Backdrop",
                table: "Album",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlbumGenre",
                columns: table => new
                {
                    AlbumsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumGenre", x => new { x.AlbumsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_AlbumGenre_Album_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumGenre_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BandGenre",
                columns: table => new
                {
                    BandsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandGenre", x => new { x.BandsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BandGenre_Band_BandsId",
                        column: x => x.BandsId,
                        principalTable: "Band",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandGenre_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GenreMusic",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MusicsId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMusic", x => new { x.GenresId, x.MusicsId });
                    table.ForeignKey(
                        name: "FK_GenreMusic_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMusic_Music_MusicsId",
                        column: x => x.MusicsId,
                        principalTable: "Music",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GenrePlaylist",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlaylistsId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenrePlaylist", x => new { x.GenresId, x.PlaylistsId });
                    table.ForeignKey(
                        name: "FK_GenrePlaylist_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenrePlaylist_Playlist_PlaylistsId",
                        column: x => x.PlaylistsId,
                        principalTable: "Playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumGenre_GenresId",
                table: "AlbumGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_BandGenre_GenresId",
                table: "BandGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMusic_MusicsId",
                table: "GenreMusic",
                column: "MusicsId");

            migrationBuilder.CreateIndex(
                name: "IX_GenrePlaylist_PlaylistsId",
                table: "GenrePlaylist",
                column: "PlaylistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumGenre");

            migrationBuilder.DropTable(
                name: "BandGenre");

            migrationBuilder.DropTable(
                name: "GenreMusic");

            migrationBuilder.DropTable(
                name: "GenrePlaylist");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropColumn(
                name: "Backdrop",
                table: "Playlist");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "Backdrop",
                table: "Album");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "User",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 0, DateTimeKind.Local).AddTicks(3502),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 10, DateTimeKind.Local).AddTicks(2687));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "PlaylistPersonal",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 23, 990, DateTimeKind.Local).AddTicks(5589),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 4, DateTimeKind.Local).AddTicks(3398));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayListPersonal",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 23, 997, DateTimeKind.Local).AddTicks(3297),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 8, DateTimeKind.Local).AddTicks(9706));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayList",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 41, DateTimeKind.Local).AddTicks(4891),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 54, DateTimeKind.Local).AddTicks(5161));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatPlayList",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 46, DateTimeKind.Local).AddTicks(8138),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 61, DateTimeKind.Local).AddTicks(2152));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatMusic",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 33, DateTimeKind.Local).AddTicks(932),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 45, DateTimeKind.Local).AddTicks(6478));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatAlbum",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 8, 22, 57, 24, 14, DateTimeKind.Local).AddTicks(8631),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 3, 18, 20, 31, 24, 29, DateTimeKind.Local).AddTicks(5382));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Band",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Backdrop",
                table: "Band",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }
    }
}
