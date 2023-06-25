using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TranslationManagement.Bll.Models;
using TranslationManagement.Bll.Models.TransactionJob;
using TranslationManagement.Bll.Services.TranslationJobFileReader;
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
        private readonly TranslationJobFileReaderFactory _translationJobFileReaderFactory;

        public TranslationJobService(ILogger<TranslationJobService> logger, AppDbContext ctx, TranslationJobFileReaderFactory translationJobFileReaderFactory)
        {
            _logger = logger;
            _context = ctx;
            _translationJobFileReaderFactory = translationJobFileReaderFactory;
        }

        public TranslationJob[] GetJobs()
        {
            return _context.TranslationJobs.ToArray();
        }
        public async Task<TranslationJob> CreateJobAsync(CreateTranslationJobModel createJobModel)
        {
            var jobEntity = new TranslationJob()
            {
                Status = JobStatusEnum.New,
                CustomerName = createJobModel.CustomerName,
                OriginalContent = createJobModel.OriginalContent,
            };
            jobEntity.SetPrice(PricePerCharacter);
            _context.TranslationJobs.Add(jobEntity);
            bool success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                var notificationSvc = new UnreliableNotificationService();
                while (!notificationSvc.SendNotification($"Job created: {jobEntity.Id}").Result)
                {
                    // at least release the thread
                    // TODO - refactor using message queue (eg. rabbitMQ)
                    await Task.Delay(1000); 
                }

                _logger.LogInformation("New job notification sent");
                return jobEntity;
            }

            return null;
        }

        public async Task<TranslationJob> CreateJobWithFileAsync(CreateTranslationJobFileModel translationJobFileModel)
        {
            var fileReader = _translationJobFileReaderFactory.GetTranslationJobFileReader(translationJobFileModel.FileName);
            var jobModel = fileReader.ReadFile(translationJobFileModel);

            return await CreateJobAsync(jobModel);
        }

        public ActionResultModel<TranslationJob> UpdateJobStatus(UpdateTranslationJobStatusModel transactionJobStatusModel)
        {
            _logger.LogInformation($"Job status update request received: {transactionJobStatusModel.NewStatus} for job {transactionJobStatusModel.JobId} by translator {transactionJobStatusModel.TranslatorId}");

            var job = _context.TranslationJobs.SingleOrDefault(j => j.Id == transactionJobStatusModel.JobId);
            if (job == null)
                return new ActionResultModel<TranslationJob>(false) { Message = $"Translation job with id {transactionJobStatusModel.JobId} not found" };

            if (job.SetStatus(transactionJobStatusModel.NewStatus))
            {
                _context.SaveChanges();
                return new ActionResultModel<TranslationJob>(true) { Result = job };
            }

            return new ActionResultModel<TranslationJob>(false)
            {
                Result = job,
                Message = $"Invalid job status change from {job.Status} to {transactionJobStatusModel.NewStatus}"
            };
        }
    }
}
