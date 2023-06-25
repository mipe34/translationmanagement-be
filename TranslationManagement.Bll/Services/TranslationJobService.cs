using External.ThirdParty.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TranslationManagement.Bll.Models.TransactionJob;
using TranslationManagement.Dal;
using TranslationManagement.Dal.Enums;
using TranslationManagement.Dal.Models;

namespace TranslationManagement.Bll.Services
{
    public class TranslationJobService
    {
        // TODO - Move to app settings configuration or (better) create same configuration in db
        const float PricePerCharacter = 0.01f;

        private readonly AppDbContext _context;
        private readonly ILogger<TranslationJobService> _logger;

        public TranslationJobService(ILogger<TranslationJobService> logger, AppDbContext ctx)
        {
            if(ctx == null) throw new ArgumentNullException(nameof(ctx));
            _context = ctx;
        }

        public TranslationJob[] GetJobs()
        {
            return _context.TranslationJobs.ToArray();
        }   
        public bool CreateJob(TranslationJob job)
        {
            job.Status = JobStatusEnum.New;
            SetPrice(job);
            _context.TranslationJobs.Add(job);
            bool success = _context.SaveChanges() > 0;
            if (success)
            {
                var notificationSvc = new UnreliableNotificationService();
                //TODO refactor calling notification service more effectively
                while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
                {
                }

                _logger.LogInformation("New job notification sent");
            }

            return success;
        }

        public bool CreateJobWithFile(CreateTransactionJobFileModel transactionJobFileModel)
        {
            using var reader = new StreamReader(transactionJobFileModel.FileStream);
            string content;
            string customerName = null;
            // TODO refactor with factory 
            if (transactionJobFileModel.FileName.EndsWith(".txt"))
            {
                content = reader.ReadToEnd();
            }
            else if (transactionJobFileModel.FileName.EndsWith(".xml"))
            {
                var xdoc = XDocument.Parse(reader.ReadToEnd());
                content = xdoc.Root.Element("Content").Value;
                customerName = xdoc.Root.Element("Customer").Value.Trim();
            }
            else
            {
                throw new NotSupportedException("unsupported file");
            }

            var newJob = new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = string.Empty,
                CustomerName = customerName ?? transactionJobFileModel.CustomerName,
            };

            SetPrice(newJob);

            return CreateJob(newJob);
        }

        public string UpdateJobStatus(UpdateTransactionJobStatusModel transactionJobStatusModel)
        {
            _logger.LogInformation($"Job status update request received: {transactionJobStatusModel.NewStatus} for job {transactionJobStatusModel.JobId} by translator {transactionJobStatusModel.TranslatorId}");
            //if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
            //{
            //    return "invalid status";
            //}

            var job = _context.TranslationJobs.Single(j => j.Id == transactionJobStatusModel.JobId);

            bool isInvalidStatusChange = (job.Status == JobStatusEnum.New && transactionJobStatusModel.NewStatus == JobStatusEnum.Completed) ||
                                         job.Status == JobStatusEnum.Completed || transactionJobStatusModel.NewStatus == JobStatusEnum.New;
            if (isInvalidStatusChange)
            {
                return "invalid status change";
            }

            job.Status = transactionJobStatusModel.NewStatus;
            _context.SaveChanges();
            return "updated";
        }

        internal void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }
    }
}
