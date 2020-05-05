using Microsoft.EntityFrameworkCore.Migrations;

namespace CordEstates.Migrations
{
#pragma warning disable IDE1006 // Naming Styles
    public partial class customerPropertiesAdded : Migration
#pragma warning restore IDE1006 // Naming Styles
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Customers_CustomerId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CustomerId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Listings");

            migrationBuilder.CreateTable(
                name: "CustomerProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    PropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProperty_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProperty_Listings_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProperty_CustomerId",
                table: "CustomerProperty",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProperty_PropertyId",
                table: "CustomerProperty",
                column: "PropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProperty");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CustomerId",
                table: "Listings",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Customers_CustomerId",
                table: "Listings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
