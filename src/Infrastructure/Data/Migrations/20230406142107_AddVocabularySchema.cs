using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class AddVocabularySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "word",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "varchar(10)", nullable: false),
                    translation = table.Column<string>(type: "varchar(100)", nullable: true),
                    pronunciation = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_word", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "word_reading",
                columns: table => new
                {
                    word_id = table.Column<int>(type: "integer", nullable: false),
                    reading_id = table.Column<int>(type: "integer", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_word_reading", x => new { x.word_id, x.reading_id, x.order });
                    table.ForeignKey(
                        name: "fk_word_reading_reading_reading_id",
                        column: x => x.reading_id,
                        principalTable: "reading",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_word_reading_word_word_id",
                        column: x => x.word_id,
                        principalTable: "word",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_word_reading_reading_id",
                table: "word_reading",
                column: "reading_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "word_reading");

            migrationBuilder.DropTable(
                name: "word");
        }
    }
}
