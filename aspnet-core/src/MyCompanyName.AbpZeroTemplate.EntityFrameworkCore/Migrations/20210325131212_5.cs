using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AttachmentEntityTypes",
                columns: new[] { "Id", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "Folder", "IsDeleted", "LastModificationTime", "LastModifierUserId", "ParentTypeId", "TenantId" },
                values: new object[] { 1, "شخص", new DateTime(2021, 3, 25, 15, 12, 11, 835, DateTimeKind.Local).AddTicks(2498), null, null, null, "Person", "/Attachments/persons", false, null, null, null, null });

            migrationBuilder.InsertData(
                table: "AttachmentTypes",
                columns: new[] { "Id", "AllowedExtensions", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "EntityTypeId", "IsDeleted", "LastModificationTime", "LastModifierUserId", "MaxAttachments", "MaxSize", "TenantId" },
                values: new object[] { 1, "jpg,png", "صورة الهوية", new DateTime(2021, 3, 25, 15, 12, 11, 831, DateTimeKind.Local).AddTicks(2100), null, null, null, "ID Card Image", 1, false, null, null, 0, 0, null });

            migrationBuilder.CreateIndex(
                name: "IX_persons_TenantId",
                table: "persons",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DeleteData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
