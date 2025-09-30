using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerSubscriptionApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToCustomerSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMasters",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerSubscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerSubscriptions");

            migrationBuilder.InsertData(
                table: "UserMasters",
                columns: new[] { "UserId", "Email", "PasswordHash", "Role", "Status", "UserName" },
                values: new object[] { 1, "admin@local.test", "AQAAAAIAAYagAAAAEANdqp4/XQm26CNpZXKQr7W454ug+x1B2HNWpoIHXM4qBhHYqs12lNFIHq/M0DYhwQ==", "Admin", 1, "admin@local.test" });
        }
    }
}
