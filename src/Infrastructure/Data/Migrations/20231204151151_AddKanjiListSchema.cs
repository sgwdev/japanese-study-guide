using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddKanjiListSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kanji_list",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kanji_list", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kanji_list_kanji",
                columns: table => new
                {
                    kanji_list_id = table.Column<int>(type: "integer", nullable: false),
                    kanji_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kanji_list_kanji", x => new { x.kanji_id, x.kanji_list_id });
                    table.ForeignKey(
                        name: "fk_kanji_list_kanji_kanji_kanji_id",
                        column: x => x.kanji_id,
                        principalTable: "kanji",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kanji_list_kanji_kanji_list_kanji_list_id",
                        column: x => x.kanji_list_id,
                        principalTable: "kanji_list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_kanji_list_kanji_kanji_list_id",
                table: "kanji_list_kanji",
                column: "kanji_list_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kanji_list_kanji");

            migrationBuilder.DropTable(
                name: "kanji_list");
        }
    }
}
