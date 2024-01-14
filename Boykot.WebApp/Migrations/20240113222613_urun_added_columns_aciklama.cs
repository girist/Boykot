using Microsoft.EntityFrameworkCore.Migrations;

namespace Boykot.WebApp.Migrations
{
    public partial class urun_added_columns_aciklama : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Aktifmi",
                table: "Uruns",
                type: "bit",
                nullable: false,
                defaultValueSql: "CONVERT([bit],(1))",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama1",
                table: "Uruns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Aciklama2",
                table: "Uruns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Aciklama3",
                table: "Uruns",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama1",
                table: "Uruns");

            migrationBuilder.DropColumn(
                name: "Aciklama2",
                table: "Uruns");

            migrationBuilder.DropColumn(
                name: "Aciklama3",
                table: "Uruns");

            migrationBuilder.AlterColumn<bool>(
                name: "Aktifmi",
                table: "Uruns",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "CONVERT([bit],(1))");
        }
    }
}
