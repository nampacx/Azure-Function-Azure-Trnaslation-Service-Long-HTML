using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TranslatorIntegration.Services.Implementation;
using TrnaslatorIntegration.Services.Interfaces;

[assembly: FunctionsStartup(typeof(TranslatorIntegration.Startup))]
namespace TranslatorIntegration
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _config;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(l =>
            {
                var aiik = _config.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");
                if (!string.IsNullOrEmpty(aiik))
                {
                    l.AddApplicationInsights(aiik, ai =>
                    {
                        ai.TrackExceptionsAsExceptionTelemetry = true;
                        ai.IncludeScopes = true;
                        ai.FlushOnDispose = true;
                    });
                }
                l.AddDebug();
                l.AddConsole();
            });

            builder.Services.AddTransient<ITranslator>(p =>
            {
                var tokeUir = _config.GetValue<string>("tokenUri");
                var token = _config.GetValue<string>("translatorTokenKey");
                var translatorBasUri = _config.GetValue<string>("translatorBaseuri");

                return new Translator(tokeUir, token, translatorBasUri);
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            var env = context.EnvironmentName;

            // add azure configuration
            if (env.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                builder.ConfigurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);
            }
            _config = builder.ConfigurationBuilder.Build();
        }

    }
}
