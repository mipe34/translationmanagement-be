using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Bll.Models.TransactionJob;

namespace TranslationManagement.Bll.Services.TranslationJobFileReader
{
    public interface ITranslationJobFileReader
    {
        CreateTranslationJobModel ReadFile(CreateTranslationJobFileModel createTranslationJobFileModel);
    }
}
