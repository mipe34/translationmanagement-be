using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Bll.Models
{
    public class ActionResultModel<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }

        public ActionResultModel(bool success)
        {
            Success = success;
        }
    }
}
