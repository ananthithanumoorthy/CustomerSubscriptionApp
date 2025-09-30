using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerSubscriptionApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerSubscriptions_CustomerId_SubscriptionName",
                table: "CustomerSubscriptions");

            migrationBuilder.InsertData(
                table: "UserMasters",
                columns: new[] { "UserId", "Email", "PasswordHash", "Role", "Status", "UserName" },
                values: new object[] { 1, "admin@local.test", "AQAAAAIAAYagAAAAEANdqp4/XQm26CNpZXKQr7W454ug+x1B2HNWpoIHXM4qBhHYqs12lNFIHq/M0DYhwQ==", "Admin", 1, "admin@local.test" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSubscriptions_CustomerId_SubscriptionName",
                table: "CustomerSubscriptions",
                columns: new[] { "CustomerId", "SubscriptionName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerSubscriptions_CustomerId_SubscriptionName",
                table: "CustomerSubscriptions");

            migrationBuilder.DeleteData(
                table: "UserMasters",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSubscriptions_CustomerId_SubscriptionName",
                table: "CustomerSubscriptions",
                columns: new[] { "CustomerId", "SubscriptionName" });
        }
    }
}
