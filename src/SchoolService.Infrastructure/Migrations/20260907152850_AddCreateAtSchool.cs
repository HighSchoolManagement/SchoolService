using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateAtSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolCode = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_School_SchoolCode",
                table: "School",
                column: "SchoolCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "School");
        }
    }
}
