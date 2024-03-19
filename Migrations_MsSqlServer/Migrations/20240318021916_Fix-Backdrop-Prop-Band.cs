using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations_MsSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class FixBackdropPropBand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 788, DateTimeKind.Local).AddTicks(9297),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 536, DateTimeKind.Local).AddTicks(9399));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "PlaylistPersonal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 782, DateTimeKind.Local).AddTicks(9728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 527, DateTimeKind.Local).AddTicks(932));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayListPersonal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 787, DateTimeKind.Local).AddTicks(5981),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 534, DateTimeKind.Local).AddTicks(7256));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 847, DateTimeKind.Local).AddTicks(5438),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 686, DateTimeKind.Local).AddTicks(4710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatPlayList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 854, DateTimeKind.Local).AddTicks(8449),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 695, DateTimeKind.Local).AddTicks(8989));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatMusic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 841, DateTimeKind.Local).AddTicks(8944),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 671, DateTimeKind.Local).AddTicks(7309));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatAlbum",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 819, DateTimeKind.Local).AddTicks(7888),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 604, DateTimeKind.Local).AddTicks(9999));

            migrationBuilder.AlterColumn<string>(
                name: "Backdrop",
                table: "Band",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 536, DateTimeKind.Local).AddTicks(9399),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 788, DateTimeKind.Local).AddTicks(9297));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtCreated",
                table: "PlaylistPersonal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 527, DateTimeKind.Local).AddTicks(932),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 782, DateTimeKind.Local).AddTicks(9728));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayListPersonal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 534, DateTimeKind.Local).AddTicks(7256),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 787, DateTimeKind.Local).AddTicks(5981));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "MusicPlayList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 686, DateTimeKind.Local).AddTicks(4710),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 847, DateTimeKind.Local).AddTicks(5438));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatPlayList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 695, DateTimeKind.Local).AddTicks(8989),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 854, DateTimeKind.Local).AddTicks(8449));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatMusic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 671, DateTimeKind.Local).AddTicks(7309),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 841, DateTimeKind.Local).AddTicks(8944));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DtAdded",
                table: "FlatAlbum",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 3, 17, 20, 49, 13, 604, DateTimeKind.Local).AddTicks(9999),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 3, 17, 23, 19, 13, 819, DateTimeKind.Local).AddTicks(7888));

            migrationBuilder.AlterColumn<string>(
                name: "Backdrop",
                table: "Band",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
