using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Dal.Enums;

namespace TranslationManagement.Bll.Models.TransactionJob
{
    public class UpdateTransactionJobStatusModel
    {
        public int JobId { get; set; }
        
        public int TranslatorId { get; set; }
      
        public JobStatusEnum NewStatus { get; set; }

        public UpdateTransactionJobStatusModel(int jobId, int translatorId, JobStatusEnum newStatus)
        {
            JobId = jobId;
            TranslatorId = translatorId;
            NewStatus = newStatus;
        }
    }
}
