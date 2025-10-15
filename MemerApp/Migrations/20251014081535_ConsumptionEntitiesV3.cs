using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemerApp.Migrations
{
    /// <inheritdoc />
    public partial class ConsumptionEntitiesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "ConsumptionLines",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LineTotal",
                table: "ConsumptionLines",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LineSubtotal",
                table: "ConsumptionLines",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "ConsumptionLines",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CouponValue",
                table: "ConsumptionLines",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "ConsumptionLines",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LineTotal",
                table: "ConsumptionLines",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LineSubtotal",
                table: "ConsumptionLines",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "ConsumptionLines",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CouponValue",
                table: "ConsumptionLines",
                type: "decimal(12,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);
        }
    }
}
