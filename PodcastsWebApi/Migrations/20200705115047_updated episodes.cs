using Microsoft.EntityFrameworkCore.Migrations;

namespace PodcastsWebApi.Migrations
{
    public partial class updatedepisodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Podcasts_PodcastId",
                table: "Episodes");

            migrationBuilder.AlterColumn<long>(
                name: "PodcastId",
                table: "Episodes",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PubDate",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Podcasts_PodcastId",
                table: "Episodes",
                column: "PodcastId",
                principalTable: "Podcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Podcasts_PodcastId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "PubDate",
                table: "Episodes");

            migrationBuilder.AlterColumn<long>(
                name: "PodcastId",
                table: "Episodes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Podcasts_PodcastId",
                table: "Episodes",
                column: "PodcastId",
                principalTable: "Podcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
