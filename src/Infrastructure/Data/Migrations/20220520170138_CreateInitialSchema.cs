using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class CreateInitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kanji",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kanji", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kanji_id = table.Column<int>(type: "integer", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading", x => x.id);
                    table.ForeignKey(
                        name: "fk_reading_kanji_kanji_id",
                        column: x => x.kanji_id,
                        principalTable: "kanji",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reading_reading_type_type_id",
                        column: x => x.type_id,
                        principalTable: "reading_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "reading_type",
                columns: new[] { "id", "label" },
                values: new object[,]
                {
                    { 1, "On" },
                    { 2, "Kun" },
                    { 3, "Special" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_kanji_character",
                table: "kanji",
                column: "character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reading_kanji_id",
                table: "reading",
                column: "kanji_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_type_id",
                table: "reading",
                column: "type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reading");

            migrationBuilder.DropTable(
                name: "kanji");

            migrationBuilder.DropTable(
                name: "reading_type");
        }
    }
}
