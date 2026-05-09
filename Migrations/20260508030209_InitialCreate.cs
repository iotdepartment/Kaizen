using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kaizen.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    NoEmpleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.NoEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "KaizenIdeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoEmpleado = table.Column<int>(type: "int", nullable: false),
                    Supervisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Turno = table.Column<int>(type: "int", nullable: false),
                    AreaAplicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagenActualBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagenMejoradaBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KaizenIdeas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KaizenIdeas_Empleados_NoEmpleado",
                        column: x => x.NoEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "NoEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KaizenIdeas_NoEmpleado",
                table: "KaizenIdeas",
                column: "NoEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KaizenIdeas");

            migrationBuilder.DropTable(
                name: "Empleados");
        }
    }
}
