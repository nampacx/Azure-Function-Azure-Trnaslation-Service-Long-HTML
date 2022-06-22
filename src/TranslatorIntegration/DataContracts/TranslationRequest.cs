using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorIntegration.DataContracts
{
    public class TranslationRequest
    {
        public string HTMLContent { get; set; }

        public string FromLang { get; set; }
        public string  ToLang { get; set; } = "en";
    }
}
