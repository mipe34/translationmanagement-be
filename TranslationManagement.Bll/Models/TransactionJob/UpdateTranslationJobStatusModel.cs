using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Bll.Models.TransactionJob
{
    public class UpdateTranslationJobStatusModel
    {
        public int JobId { get; set; }
        
        public int TranslatorId { get; set; }
      
        public JobStatusEnum NewStatus { get; set; }
    }
}
