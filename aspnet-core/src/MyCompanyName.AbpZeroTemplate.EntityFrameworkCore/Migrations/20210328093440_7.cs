using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileToken",
                table: "AttachmentFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 12, 34, 39, 617, DateTimeKind.Local).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 12, 34, 39, 614, DateTimeKind.Local).AddTicks(1633));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileToken",
                table: "AttachmentFiles");

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 9, 50, 13, 747, DateTimeKind.Local).AddTicks(1257));

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 9, 50, 13, 743, DateTimeKind.Local).AddTicks(6900));
        }
    }
}
