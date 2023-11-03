using Microsoft.EntityFrameworkCore.Migrations;

namespace Boykot.WebApp.Migrations
{
    public partial class mig_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uruns_Kategoris_KategoriId",
                table: "Uruns");

            migrationBuilder.DropTable(
                name: "Kategoris");

            migrationBuilder.DropIndex(
                name: "IX_Uruns_KategoriId",
                table: "Uruns");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Uruns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Uruns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kategoris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktifmi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoris", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uruns_KategoriId",
                table: "Uruns",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uruns_Kategoris_KategoriId",
                table: "Uruns",
                column: "KategoriId",
                principalTable: "Kategoris",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
