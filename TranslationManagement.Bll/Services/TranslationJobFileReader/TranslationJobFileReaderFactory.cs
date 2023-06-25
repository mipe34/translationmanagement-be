using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Bll.Services.TranslationJobFileReader
{
    public class TranslationJobFileReaderFactory
    {
        private Dictionary<string, ITranslationJobFileReader> fileReaders = new Dictionary<string, ITranslationJobFileReader>
        {
            {"txt", new PlainTextTranslationJobFileReader() },
            {"xml", new XmlTranslationJobFileReader() }
        };

        public ITranslationJobFileReader GetTranslationJobFileReader(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            if (fileReaders.ContainsKey(fileExtension))
            {
                return fileReaders[fileExtension];
            }
            throw new NotSupportedException($"File with extension {fileExtension} not supported");
        }
    }
}
