using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTNConecta.API.Migrations
{
    /// <inheritdoc />
    public partial class EmailUnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Organizaciones_Email",
                table: "Organizaciones",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_graduado_email",
                table: "graduado",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizaciones_Email",
                table: "Organizaciones");

            migrationBuilder.DropIndex(
                name: "IX_graduado_email",
                table: "graduado");
        }
    }
}
