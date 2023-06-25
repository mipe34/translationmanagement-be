using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Api.Models.TranslationJob;
using TranslationManagement.Bll.Models.TransactionJob;
using TranslationManagement.Bll.Services;
using TranslationManagement.Dal.Models;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TranslationJobService _translationJobService;


        public TranslationJobController(IMapper mapper, TranslationJobService translationJobService)
        {
            _mapper = mapper;
            _translationJobService = translationJobService;
        }

        [HttpGet]
        public ActionResult GetJobs()
        {
            var dto = _mapper.Map<TranslationJob[], TranslationJobDto[]>(_translationJobService.GetJobs());
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateJob(CreateTranslationJobDto jobDto)
        {
            var jobModel = _mapper.Map<CreateTranslationJobModel>(jobDto);
            var result = await _translationJobService.CreateJobAsync(jobModel);
            if(result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Cannot create job.");
            return Ok(result);
        }

        [HttpPost]
        [Route("createJobWithFile")]
        public async Task<ActionResult> CreateJobWithFile(IFormFile file, string customer)
        {
            using var stream = file.OpenReadStream();
            var transactionJobModel = new CreateTranslationJobFileModel(file.FileName, stream) { CustomerName = customer };
            try
            {
                var result = await _translationJobService.CreateJobWithFileAsync(transactionJobModel);
                if(result == null) return StatusCode(StatusCodes.Status500InternalServerError, "Cannot create job.");
                return Ok(result);
            }
            catch(NotSupportedException ex)
            {
                return BadRequest($"Cannot create job. Message: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("status")]
        public ActionResult UpdateJobStatus(UpdateTranslationJobStatusDto jobDto)
        {
            var model = _mapper.Map<UpdateTranslationJobStatusModel>(jobDto);
            var result = _translationJobService.UpdateJobStatus(model);
            if (result.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.Message);
        }
    }
}