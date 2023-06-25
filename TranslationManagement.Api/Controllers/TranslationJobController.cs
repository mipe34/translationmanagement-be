using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Bll.Models.TransactionJob;
using TranslationManagement.Bll.Services;
using TranslationManagement.Dal;
using TranslationManagement.Dal.Enums;
using TranslationManagement.Dal.Models;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private AppDbContext _context;
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly TranslationJobService _translationJobService;

        public TranslationJobController(ILogger<TranslatorManagementController> logger, TranslationJobService translationJobService)
        {
            _logger = logger;
            _translationJobService = translationJobService;
        }

        [HttpGet]
        public TranslationJob[] GetJobs()
        {
            return _translationJobService.GetJobs();
        }

        [HttpPost]
        public bool CreateJob(TranslationJob job)
        {
            return _translationJobService.CreateJob(job);
        }

        [HttpPost]
        public bool CreateJobWithFile(IFormFile file, string customer)
        {
            using var stream = file.OpenReadStream();
            var transactionJobModel = new CreateTransactionJobFileModel(file.FileName, stream) { CustomerName = customer };
            var result = _translationJobService.CreateJobWithFile(transactionJobModel);
            return result;
        }

        [HttpPost]
        public string UpdateJobStatus(int jobId, int translatorId, JobStatusEnum newStatus)
        {
            return _translationJobService.UpdateJobStatus(new UpdateTransactionJobStatusModel(jobId, translatorId, newStatus));
        }
    }
}