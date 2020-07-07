using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Authorization.Migrations
{
    public partial class AddUserPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserAuthEntities",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserAuthEntities_PhoneNumber",
                table: "UserAuthEntities",
                column: "PhoneNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserAuthEntities_UserUid",
                table: "UserAuthEntities",
                column: "UserUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserAuthEntities_PhoneNumber",
                table: "UserAuthEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserAuthEntities_UserUid",
                table: "UserAuthEntities");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserAuthEntities");
        }
    }
}
