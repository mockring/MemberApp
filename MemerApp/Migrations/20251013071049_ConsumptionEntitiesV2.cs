using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemerApp.Migrations
{
    /// <inheritdoc />
    public partial class ConsumptionEntitiesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Coupons",
                newName: "CouponValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CouponValue",
                table: "Coupons",
                newName: "DiscountValue");
        }
    }
}
