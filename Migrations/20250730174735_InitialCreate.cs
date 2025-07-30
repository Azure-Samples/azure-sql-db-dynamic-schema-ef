using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Azure.SQLDB.Samples.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "global_sequence");

            migrationBuilder.CreateTable(
                name: "todo_hybrid",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "next value for dbo.global_sequence"),
                    todo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    completed = table.Column<byte>(type: "tinyint", nullable: false),
                    extension = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_hybrid", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo_hybrid");

            migrationBuilder.DropSequence(
                name: "global_sequence");
        }
    }
}
