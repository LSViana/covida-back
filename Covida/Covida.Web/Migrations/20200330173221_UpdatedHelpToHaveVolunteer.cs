using Microsoft.EntityFrameworkCore.Migrations;

namespace Covida.Web.Migrations
{
    public partial class UpdatedHelpToHaveVolunteer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Helps_Users_UserId",
                table: "Helps");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Helps_HelpId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_UserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Helps_UserId",
                table: "Helps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Helps");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_HelpId",
                table: "Messages",
                newName: "IX_Messages_HelpId");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Helps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VolunteerId",
                table: "Helps",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Helps_AuthorId",
                table: "Helps",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Helps_VolunteerId",
                table: "Helps",
                column: "VolunteerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Helps_Users_AuthorId",
                table: "Helps",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Helps_Users_VolunteerId",
                table: "Helps",
                column: "VolunteerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Helps_HelpId",
                table: "Messages",
                column: "HelpId",
                principalTable: "Helps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Helps_Users_AuthorId",
                table: "Helps");

            migrationBuilder.DropForeignKey(
                name: "FK_Helps_Users_VolunteerId",
                table: "Helps");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Helps_HelpId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Helps_AuthorId",
                table: "Helps");

            migrationBuilder.DropIndex(
                name: "IX_Helps_VolunteerId",
                table: "Helps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Helps");

            migrationBuilder.DropColumn(
                name: "VolunteerId",
                table: "Helps");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Message",
                newName: "IX_Message_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_HelpId",
                table: "Message",
                newName: "IX_Message_HelpId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Helps",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Helps_UserId",
                table: "Helps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Helps_Users_UserId",
                table: "Helps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Helps_HelpId",
                table: "Message",
                column: "HelpId",
                principalTable: "Helps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
