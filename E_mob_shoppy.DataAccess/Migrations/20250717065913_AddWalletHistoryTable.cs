using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_mob_shoppy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletHistories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "StartDateTime",
                value: new DateTime(2025, 7, 17, 12, 29, 13, 113, DateTimeKind.Local).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "StartDateTime",
                value: new DateTime(2025, 7, 17, 12, 29, 13, 113, DateTimeKind.Local).AddTicks(9196));

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_ApplicationUserId",
                table: "WalletHistories",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletHistories");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "StartDateTime",
                value: new DateTime(2025, 7, 8, 10, 44, 34, 746, DateTimeKind.Local).AddTicks(5417));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "StartDateTime",
                value: new DateTime(2025, 7, 8, 10, 44, 34, 746, DateTimeKind.Local).AddTicks(5432));
        }
    }
}
