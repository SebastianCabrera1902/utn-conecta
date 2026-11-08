using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTNConecta.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPostulaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
        
        
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGraduado",
                table: "Postulaciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOportunidad",
                table: "Postulaciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
