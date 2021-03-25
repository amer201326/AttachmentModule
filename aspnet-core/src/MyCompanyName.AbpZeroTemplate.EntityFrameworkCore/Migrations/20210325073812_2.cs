using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentEntityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Folder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentTypeId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AttachmentEntityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentEntityTypes_AttachmentEntityTypes_ParentTypeId",
                        column: x => x.ParentTypeId,
                        principalTable: "AttachmentEntityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_TestEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSize = table.Column<int>(type: "int", nullable: false),
                    AllowedExtensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxAttachments = table.Column<int>(type: "int", nullable: false),
                    EntityTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AttachmentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentTypes_AttachmentEntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "AttachmentEntityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PhysicalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ObjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AttachmentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_AttachmentTypes_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntityTypes_ParentTypeId",
                table: "AttachmentEntityTypes",
                column: "ParentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntityTypes_TenantId",
                table: "AttachmentEntityTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_AttachmentTypeId",
                table: "AttachmentFiles",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_TenantId",
                table: "AttachmentFiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentTypes_EntityTypeId",
                table: "AttachmentTypes",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentTypes_TenantId",
                table: "AttachmentTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TestEntities_TenantId",
                table: "TestEntities",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentFiles");

            migrationBuilder.DropTable(
                name: "TestEntities");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "AttachmentEntityTypes");
        }
    }
}
