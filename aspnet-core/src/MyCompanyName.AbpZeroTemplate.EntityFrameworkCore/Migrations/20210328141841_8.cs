using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diseases_persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 18, 40, 338, DateTimeKind.Local).AddTicks(4947));

            migrationBuilder.InsertData(
                table: "AttachmentEntityTypes",
                columns: new[] { "Id", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "Folder", "IsDeleted", "LastModificationTime", "LastModifierUserId", "ParentTypeId", "TenantId" },
                values: new object[] { 2, "مرض", new DateTime(2021, 3, 28, 17, 18, 40, 338, DateTimeKind.Local).AddTicks(8815), null, null, null, "Desease", "/Desease", false, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2021, 3, 28, 17, 18, 40, 328, DateTimeKind.Local).AddTicks(5040));

            migrationBuilder.InsertData(
                table: "AttachmentTypes",
                columns: new[] { "Id", "AllowedExtensions", "ArName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EnName", "EntityTypeId", "IsDeleted", "IsRequired", "LastModificationTime", "LastModifierUserId", "MaxAttachments", "MaxSize", "TenantId" },
                values: new object[] { 2, "jpg,png", "تشخيص المرض", new DateTime(2021, 3, 28, 17, 18, 40, 334, DateTimeKind.Local).AddTicks(7731), null, null, null, "Diagnose the disease", 2, false, false, null, null, 0, 0, null });

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_PersonId",
                table: "Diseases",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_TenantId",
                table: "Diseases",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DeleteData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AttachmentEntityTypes",
                keyColumn: "Id",
                keyValue: 2);

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
    }
}
