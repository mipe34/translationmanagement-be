using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Api.Models.TranslationJob
{
    public class TranslationJobDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public JobStatusEnum Status { get; set; }
        public string OriginalContent { get; set; }
        public string TranslatedContent { get; set; }
        public float Price { get; set; }
    }
}
