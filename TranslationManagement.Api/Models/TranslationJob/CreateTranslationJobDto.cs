using System.ComponentModel.DataAnnotations;
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Api.Models.TranslationJob
{
    public class CreateTranslationJobDto
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string OriginalContent { get; set; }
    }
}
