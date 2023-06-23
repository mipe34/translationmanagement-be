
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Dal.Models
{
    public class Translator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float HourlyRate { get; set; }
        public TranslatorStatusEnum Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
