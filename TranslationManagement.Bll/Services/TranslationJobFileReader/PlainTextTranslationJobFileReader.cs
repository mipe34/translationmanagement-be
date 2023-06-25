using System.IO;
using TranslationManagement.Bll.Models.TransactionJob;

namespace TranslationManagement.Bll.Services.TranslationJobFileReader
{
    public class PlainTextTranslationJobFileReader : ITranslationJobFileReader
    {
        public CreateTranslationJobModel ReadFile(CreateTranslationJobFileModel translationJobFileModel)
        {
            using var reader = new StreamReader(translationJobFileModel.FileStream);
            string content = reader.ReadToEnd();

            return new CreateTranslationJobModel() { 
                OriginalContent = content,
                CustomerName = translationJobFileModel.CustomerName
            };
        }
    }
}
