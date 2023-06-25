using AutoMapper;
using TranslationManagement.Api.Models.TranslationJob;
using TranslationManagement.Bll.Models.TransactionJob;
using TranslationManagement.Dal.Models;

namespace TranslationManagement.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TranslationJob, TranslationJobDto>();
            CreateMap<CreateTranslationJobDto, CreateTranslationJobModel>();
            CreateMap<UpdateTranslationJobStatusDto, UpdateTranslationJobStatusModel>();
        }
    }
}
