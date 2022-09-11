using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        AboutMe = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
            //        Role = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Articles",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
            //        State = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Articles", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Articles_Users_UserId",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Articles_Id",
            //    table: "Articles",
            //    column: "Id",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Articles_UserId",
            //    table: "Articles",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_Id",
            //    table: "Users",
            //    column: "Id",
            //    unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Articles");

            //migrationBuilder.DropTable(
            //    name: "Users");
        }
    }
}
