using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Authorization.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAuthEntities",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserUid = table.Column<Guid>(nullable: false),
                    AccessKey = table.Column<string>(nullable: true),
                    RefreshKey = table.Column<string>(nullable: true),
                    ExpirationTime = table.Column<string>(nullable: true),
                    TemporaryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthEntities", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthEntities");
        }
    }
}
