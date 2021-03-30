using MyCompanyName.AbpZeroTemplate.Attachments;


namespace MyCompanyName.AbpZeroTemplate.Migrations.Seed.OnCreateModel
{
    public class AttachmentEntityTypeSeed
    {
        public AttachmentEntityTypeSeed(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Attachments.AttachmentEntityType> entityTypeBuilder)
        {
            entityTypeBuilder.HasData(
                new AttachmentEntityType() {Id = AttachmentEntityTypeConsts.Person, ArName = "شخص", EnName = "Person", Folder = "/Attachments/persons" },
                new AttachmentEntityType() {Id = AttachmentEntityTypeConsts.Desease, ArName = "مرض", EnName = "Desease", Folder = "/Desease" ,ParentTypeId = AttachmentEntityTypeConsts.Person },
                new AttachmentEntityType() {Id = AttachmentEntityTypeConsts.kkkk, ArName = "kkk", EnName = "kk", Folder = "/kkk" ,ParentTypeId = AttachmentEntityTypeConsts.Person }
                );
        }
    }
}
