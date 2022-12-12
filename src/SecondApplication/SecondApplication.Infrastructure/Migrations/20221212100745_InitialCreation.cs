using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postnfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    HasFrenchTag = table.Column<bool>(type: "boolean", nullable: false),
                    HasFictonTag = table.Column<bool>(type: "boolean", nullable: false),
                    HasMoreThanTwoReactions = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postnfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postnfo");
        }
    }
}
