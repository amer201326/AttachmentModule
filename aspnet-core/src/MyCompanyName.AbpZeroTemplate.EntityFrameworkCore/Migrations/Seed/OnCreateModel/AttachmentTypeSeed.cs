using MyCompanyName.AbpZeroTemplate.Attachments;


namespace MyCompanyName.AbpZeroTemplate.Migrations.Seed.OnCreateModel
{
    public class AttachmentTypeSeed
    {
        public AttachmentTypeSeed(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Attachments.AttachmentType> entityTypeBuilder)
        {
            entityTypeBuilder.HasData(
                new AttachmentType() {Id = AttachmentTypeConsts.IDCardImageID, ArName = "صورة الهوية", EnName = "ID Card Image", AllowedExtensions = "",EntityTypeId = AttachmentEntityTypeConsts.Person},
                new AttachmentType() {Id = AttachmentTypeConsts.DiagnoseTheDisease, ArName = "تشخيص المرض", EnName = "Diagnose the disease", AllowedExtensions = "",EntityTypeId = AttachmentEntityTypeConsts.Desease},
                new AttachmentType() {Id = AttachmentTypeConsts.kkkk, ArName = "kkk", EnName = "lkkk", AllowedExtensions = "",EntityTypeId = AttachmentEntityTypeConsts.kkkk }
                );
        }
    }
}
