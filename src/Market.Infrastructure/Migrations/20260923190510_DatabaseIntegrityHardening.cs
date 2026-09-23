using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseIntegrityHardening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Vendors_CommissionRate_Range",
                table: "Vendors",
                sql: "[CommissionRate] >= 0.00 AND [CommissionRate] <= 0.50");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId_ProductId",
                table: "Reviews",
                columns: new[] { "CustomerId", "ProductId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reviews_RatingValue_Range",
                table: "Reviews",
                sql: "[RatingValue] >= 1 AND [RatingValue] <= 5");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId_VendorId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId", "VendorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Vendors_CommissionRate_Range",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CustomerId_ProductId",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reviews_RatingValue_Range",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId_ProductId_VendorId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);
        }
    }
}
