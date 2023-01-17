﻿//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace Blog.Persistence.Migrations
//{
//    public partial class addMessageEntity : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Messages",
//                columns: table => new
//                {
//                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
//                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
//                    SenderUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    RecipienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
//                    RecipienUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
//                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
//                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    SenderDeleted = table.Column<bool>(type: "bit", nullable: false),
//                    RecipientDeleted = table.Column<bool>(type: "bit", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Messages", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Messages_AspNetUsers_RecipientId",
//                        column: x => x.RecipientId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Messages_AspNetUsers_SenderId",
//                        column: x => x.SenderId,
//                        principalTable: "AspNetUsers",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Messages_RecipientId",
//                table: "Messages",
//                column: "RecipientId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Messages_SenderId",
//                table: "Messages",
//                column: "SenderId");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Messages");
//        }
//    }
//}
