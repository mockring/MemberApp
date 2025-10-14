using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemerApp.Migrations
{
    /// <inheritdoc />
    public partial class ConsumptionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumptions",
                columns: table => new
                {
                    ConsumptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MemberPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalBeforeDiscount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TotalAfterDiscount = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumptions", x => x.ConsumptionId);
                });

            migrationBuilder.CreateTable(
                name: "ConsumptionLines",
                columns: table => new
                {
                    ConsumptionLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsumptionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LineSubtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CouponId = table.Column<int>(type: "int", nullable: true),
                    CouponName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CouponMethod = table.Column<int>(type: "int", nullable: true),
                    CouponValue = table.Column<decimal>(type: "decimal(12,4)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionLines", x => x.ConsumptionLineId);
                    table.ForeignKey(
                        name: "FK_ConsumptionLines_Consumptions_ConsumptionId",
                        column: x => x.ConsumptionId,
                        principalTable: "Consumptions",
                        principalColumn: "ConsumptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionLines_ConsumptionId",
                table: "ConsumptionLines",
                column: "ConsumptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumptionLines");

            migrationBuilder.DropTable(
                name: "Consumptions");
        }
    }
}
