using System.ComponentModel.DataAnnotations;
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Api.Models.TranslationJob
{
    public class UpdateTranslationJobStatusDto
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        public int TranslatorId { get; set; }
        [Required]
        public JobStatusEnum NewStatus { get; set; }
    }
}
