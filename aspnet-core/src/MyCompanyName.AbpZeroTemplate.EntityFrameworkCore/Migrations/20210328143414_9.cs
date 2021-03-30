using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 34, 13, 185, DateTimeKind.Local).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "ParentTypeId" },
                values: new object[] { new DateTime(2021, 3, 28, 17, 34, 13, 185, DateTimeKind.Local).AddTicks(4204), 1 });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 34, 13, 178, DateTimeKind.Local).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 34, 13, 182, DateTimeKind.Local).AddTicks(2688));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 18, 40, 338, DateTimeKind.Local).AddTicks(4947));

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "ParentTypeId" },
                values: new object[] { new DateTime(2021, 3, 28, 17, 18, 40, 338, DateTimeKind.Local).AddTicks(8815), null });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 18, 40, 328, DateTimeKind.Local).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 18, 40, 334, DateTimeKind.Local).AddTicks(7731));
        }
    }
}
