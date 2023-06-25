using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Bll.Models.TransactionJob
{
    public class CreateTransactionJobFileModel
    {
        public string FileName { get; set; }

        public Stream FileStream { get; set; }

        public string CustomerName { get; set; }

        public CreateTransactionJobFileModel(string fileName, Stream fileStream)
        {
            FileName = fileName;
            FileStream = fileStream;
        }
    }
}
