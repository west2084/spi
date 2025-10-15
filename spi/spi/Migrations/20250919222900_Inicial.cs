using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spi.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    des_area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    siglas_area = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    des_proy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    tipo_proy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cve_obs = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    des_obs = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    indicador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fecha_obs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_cumplimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_ult_act = table.Column<DateTime>(type: "datetime2", nullable: false),
                    valor_cumplimiento = table.Column<int>(type: "int", nullable: false),
                    valor_meta = table.Column<int>(type: "int", nullable: false),
                    estatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProyectoId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observaciones_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observaciones_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evidencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObservacionesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidencias_Observaciones_ObservacionesId",
                        column: x => x.ObservacionesId,
                        principalTable: "Observaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservacionAreas",
                columns: table => new
                {
                    ObservacionesId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservacionAreas", x => new { x.ObservacionesId, x.AreaId });
                    table.ForeignKey(
                        name: "FK_ObservacionAreas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObservacionAreas_Observaciones_ObservacionesId",
                        column: x => x.ObservacionesId,
                        principalTable: "Observaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidencias_ObservacionesId",
                table: "Evidencias",
                column: "ObservacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservacionAreas_AreaId",
                table: "ObservacionAreas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Observaciones_AreaId",
                table: "Observaciones",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Observaciones_ProyectoId",
                table: "Observaciones",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AreaId",
                table: "Usuarios",
                column: "AreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidencias");

            migrationBuilder.DropTable(
                name: "ObservacionAreas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Observaciones");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
