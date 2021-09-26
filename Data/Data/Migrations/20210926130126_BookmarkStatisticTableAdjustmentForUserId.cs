using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class BookmarkStatisticTableAdjustmentForUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeenTime",
                table: "BookmarkStatistic");

            migrationBuilder.RenameColumn(
                name: "SharedByUser",
                table: "BookmarkStatistic",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BookmarkStatistic",
                newName: "SharedByUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenTime",
                table: "BookmarkStatistic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
