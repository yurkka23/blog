//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace Blog.Persistence.Migrations
//{
//    public partial class addsubscription : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "UserSubscriptions",
//                columns: table => new
//                {
//                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
//                    UserToSubscribeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_UserSubscriptions", x => new { x.UserId, x.UserToSubscribeId });
//                    table.ForeignKey(
//                        name: "FK_UserSubscriptions_AspNetUsers_UserId",
//                        column: x => x.UserId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_UserSubscriptions_AspNetUsers_UserToSubscribeId",
//                        column: x => x.UserToSubscribeId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_UserSubscriptions_UserToSubscribeId",
//                table: "UserSubscriptions",
//                column: "UserToSubscribeId");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "UserSubscriptions");
//        }
//    }
//}
