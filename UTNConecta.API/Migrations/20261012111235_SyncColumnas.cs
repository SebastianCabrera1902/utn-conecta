using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTNConecta.API.Migrations
{
    /// <inheritdoc />
    public partial class SyncColumnas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdOrganizacion",
                table: "Oportunidades",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
