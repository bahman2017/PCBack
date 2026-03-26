using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBack.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "patent_analyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatentNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Title = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    PatentOwner = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    PatentStatus = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TechnologyTags = table.Column<string>(type: "text", nullable: false),
                    PotentialMarkets = table.Column<string>(type: "text", nullable: false),
                    CommercialOpportunities = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patent_analyses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patent_analyses");
        }
    }
}
