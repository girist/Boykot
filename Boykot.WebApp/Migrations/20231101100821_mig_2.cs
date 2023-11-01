using Microsoft.EntityFrameworkCore.Migrations;

namespace Boykot.WebApp.Migrations
{
    public partial class mig_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunlers_Kategorilers_KategorilerId",
                table: "Urunlers");

            migrationBuilder.DropTable(
                name: "Kategorilers");

            migrationBuilder.DropIndex(
                name: "IX_Urunlers_KategorilerId",
                table: "Urunlers");

            migrationBuilder.DropColumn(
                name: "KategorilerId",
                table: "Urunlers");

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Urunlers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunlers_KategoriId",
                table: "Urunlers",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunlers_Kategori_KategoriId",
                table: "Urunlers",
                column: "KategoriId",
                principalTable: "Kategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunlers_Kategori_KategoriId",
                table: "Urunlers");

            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.DropIndex(
                name: "IX_Urunlers_KategoriId",
                table: "Urunlers");

            migrationBuilder.AlterColumn<int>(
                name: "KategoriId",
                table: "Urunlers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KategorilerId",
                table: "Urunlers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kategorilers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorilers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunlers_KategorilerId",
                table: "Urunlers",
                column: "KategorilerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunlers_Kategorilers_KategorilerId",
                table: "Urunlers",
                column: "KategorilerId",
                principalTable: "Kategorilers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
