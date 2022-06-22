using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace TrnaslatorIntegration.Services.Interfaces
{
    public interface ITranslator
    {
        Task<string> DetectLanguage(string input);
        Task<string> Translate(string input, string from, string to, string textType = "plain");
    }
}
