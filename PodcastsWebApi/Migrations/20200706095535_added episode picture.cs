using Microsoft.EntityFrameworkCore.Migrations;

namespace PodcastsWebApi.Migrations
{
    public partial class addedepisodepicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Episodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Episodes");
        }
    }
}
