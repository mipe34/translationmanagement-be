using System;
using System.IO;
using System.Xml.Linq;
using TranslationManagement.Bll.Models.TransactionJob;

namespace TranslationManagement.Bll.Services.TranslationJobFileReader
{
    public class XmlTranslationJobFileReader : ITranslationJobFileReader
    {
        public CreateTranslationJobModel ReadFile(CreateTranslationJobFileModel translationJobFileModel)
        {
            using var reader = new StreamReader(translationJobFileModel.FileStream);
            var xdoc = XDocument.Parse(reader.ReadToEnd());
            var content = xdoc.Root.Element("Content").Value;
            var customerName = xdoc.Root.Element("Customer").Value.Trim();

            var createTranslationJobModel = new CreateTranslationJobModel()
            {
                CustomerName = customerName ?? translationJobFileModel.CustomerName,
                OriginalContent = content
            };
            return createTranslationJobModel;
        }
    }
}
