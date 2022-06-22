using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TranslatorIntegration.DataContracts;
using TrnaslatorIntegration.Services.Interfaces;

namespace TranslatorIntegration
{
    public class TranslatorFunction
    {
        private readonly ITranslator translator;

        public TranslatorFunction(ITranslator translator)
        {
            this.translator = translator;
        }

        [FunctionName("Translate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] TranslationRequest req,
            ILogger log)
        {
            log.LogInformation("Function executed");
            var result = await this.translator.Translate(req.HTMLContent, req.FromLang, req.ToLang, "html");

            return new OkObjectResult(result);
        }
    }
}
