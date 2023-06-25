using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Bll.Models.TransactionJob
{
    public class CreateTranslationJobModel
    {
        public string CustomerName { get; set; }
        public string OriginalContent { get; set; }
    }
}
