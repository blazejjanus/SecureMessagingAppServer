using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKiK.Server.DB.Migrations
{
    /// <inheritdoc />
    public partial class DBv31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Messages_RecipientID",
                table: "Recipients");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Messages_RecipientID",
                table: "Recipients",
                column: "RecipientID",
                principalTable: "Messages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Messages_RecipientID",
                table: "Recipients");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Messages_RecipientID",
                table: "Recipients",
                column: "RecipientID",
                principalTable: "Messages",
                principalColumn: "ID");
        }
    }
}
