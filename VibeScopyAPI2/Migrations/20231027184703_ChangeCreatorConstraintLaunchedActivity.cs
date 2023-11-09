using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeScopyAPI.Migrations
{
    public partial class ChangeCreatorConstraintLaunchedActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LaunchedActivities_CreatorAuthentUid",
                table: "LaunchedActivities");

            migrationBuilder.CreateIndex(
                name: "IX_LaunchedActivities_CreatorAuthentUid",
                table: "LaunchedActivities",
                column: "CreatorAuthentUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LaunchedActivities_CreatorAuthentUid",
                table: "LaunchedActivities");

            migrationBuilder.CreateIndex(
                name: "IX_LaunchedActivities_CreatorAuthentUid",
                table: "LaunchedActivities",
                column: "CreatorAuthentUid",
                unique: true);
        }
    }
}
