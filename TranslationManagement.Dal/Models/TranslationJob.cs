
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Dal.Models
{
    public class TranslationJob
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public JobStatusEnum Status { get; set; }
        public string OriginalContent { get; set; }
        public string TranslatedContent { get; set; }
        public float Price { get; set; }

        public void SetPrice(float pricePerCharacter)
        {
            Price = OriginalContent.Length * pricePerCharacter;
        }

        public bool SetStatus(JobStatusEnum newStatus)
        {
            bool isInvalidStatusChange = (Status == JobStatusEnum.New && newStatus == JobStatusEnum.Completed) ||
                                         Status == JobStatusEnum.Completed || newStatus == JobStatusEnum.New;
            if (isInvalidStatusChange) return false;

            Status = newStatus;
            return true;
        }
    }
}
