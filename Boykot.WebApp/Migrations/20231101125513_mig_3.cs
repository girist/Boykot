using Microsoft.EntityFrameworkCore.Migrations;

namespace Boykot.WebApp.Migrations
{
    public partial class mig_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunlers_Kategori_KategoriId",
                table: "Urunlers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Urunlers",
                table: "Urunlers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategori",
                table: "Kategori");

            migrationBuilder.RenameTable(
                name: "Urunlers",
                newName: "Uruns");

            migrationBuilder.RenameTable(
                name: "Kategori",
                newName: "Kategoris");

            migrationBuilder.RenameIndex(
                name: "IX_Urunlers_KategoriId",
                table: "Uruns",
                newName: "IX_Uruns_KategoriId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Uruns",
                table: "Uruns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategoris",
                table: "Kategoris",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Uruns_Kategoris_KategoriId",
                table: "Uruns",
                column: "KategoriId",
                principalTable: "Kategoris",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uruns_Kategoris_KategoriId",
                table: "Uruns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Uruns",
                table: "Uruns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategoris",
                table: "Kategoris");

            migrationBuilder.RenameTable(
                name: "Uruns",
                newName: "Urunlers");

            migrationBuilder.RenameTable(
                name: "Kategoris",
                newName: "Kategori");

            migrationBuilder.RenameIndex(
                name: "IX_Uruns_KategoriId",
                table: "Urunlers",
                newName: "IX_Urunlers_KategoriId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urunlers",
                table: "Urunlers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategori",
                table: "Kategori",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunlers_Kategori_KategoriId",
                table: "Urunlers",
                column: "KategoriId",
                principalTable: "Kategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
