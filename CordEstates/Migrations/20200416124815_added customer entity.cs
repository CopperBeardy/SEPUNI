using Microsoft.EntityFrameworkCore.Migrations;

namespace CordEstates.Migrations
{
    public partial class addedcustomerentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Listings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Customers_CustomerId",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Listings_CustomerId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Listings");
        }
    }
}
