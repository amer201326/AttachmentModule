using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kkkks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kkkks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kkkks_persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 29, 16, 7, 41, 670, DateTimeKind.Local).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2021, 3, 29, 16, 7, 41, 670, DateTimeKind.Local).AddTicks(6769));

            migrationBuilder.InsertData(
                table: "AttachmentEntityTypes",
                columns: new[] { "Id", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "Folder", "IsDeleted", "LastModificationTime", "LastModifierUserId", "ParentTypeId", "TenantId" },
                values: new object[] { 3, "kkk", new DateTime(2021, 3, 29, 16, 7, 41, 670, DateTimeKind.Local).AddTicks(7459), null, null, null, "kk", "/kkk", false, null, null, 1, null });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowedExtensions", "CreationTime" },
                values: new object[] { "", new DateTime(2021, 3, 29, 16, 7, 41, 663, DateTimeKind.Local).AddTicks(7537) });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowedExtensions", "CreationTime" },
                values: new object[] { "", new DateTime(2021, 3, 29, 16, 7, 41, 667, DateTimeKind.Local).AddTicks(9092) });

            migrationBuilder.InsertData(
                table: "AttachmentTypes",
                columns: new[] { "Id", "AllowedExtensions", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "EntityTypeId", "IsDeleted", "IsRequired", "LastModificationTime", "LastModifierUserId", "MaxAttachments", "MaxSize", "TenantId" },
                values: new object[] { 3, "", "kkk", new DateTime(2021, 3, 29, 16, 7, 41, 667, DateTimeKind.Local).AddTicks(9179), null, null, null, "lkkk", 3, false, false, null, null, 0, 0, null });

            migrationBuilder.CreateIndex(
                name: "IX_kkkks_PersonId",
                table: "kkkks",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_kkkks_TenantId",
                table: "kkkks",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kkkks");

            migrationBuilder.DeleteData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 3);

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
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 34, 13, 185, DateTimeKind.Local).AddTicks(4204));

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowedExtensions", "CreationTime" },
                values: new object[] { "jpg,png", new DateTime(2021, 3, 28, 17, 34, 13, 178, DateTimeKind.Local).AddTicks(9456) });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowedExtensions", "CreationTime" },
                values: new object[] { "jpg,png", new DateTime(2021, 3, 28, 17, 34, 13, 182, DateTimeKind.Local).AddTicks(2688) });
        }
    }
}
