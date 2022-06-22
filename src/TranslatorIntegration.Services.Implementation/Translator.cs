using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrnaslatorIntegration.Services.Interfaces;

namespace TranslatorIntegration.Services.Implementation
{
    public class Translator : ITranslator
    {
        public Translator(string tokenUri, string translatorTokenKey, string translatorBaseuri)
        {
            this.tokenUri = tokenUri;
            this.translatorTokenKey = translatorTokenKey;
            this.translatorBaseuri = translatorBaseuri;
        }

        private readonly string tokenUri;
        private readonly string translatorTokenKey;
        private readonly string translatorBaseuri;
        private const int maxChunkSize = 5000;

        public async Task<string> DetectLanguage(string input)
        {
            var token = $"Bearer {GetToken()}";
            var body = new[] { new { text = input } };

            var client = new RestClient();
            var request = GetRequest($"{translatorBaseuri}/detect?api-version=3.0", token, body);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var detectResult = JsonConvert.DeserializeObject<List<DetectResult>>(response.Content);

                return detectResult.FirstOrDefault()?.language;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> Translate(string input, string from, string to, string textType = "plain")
        {
            var token = $"Bearer {GetToken()}";
            var body = new[] { new { text = input } };

            var client = new RestClient();
            RestRequest request = GetRequest($"{translatorBaseuri}/translate?api-version=3.0&from={from}&to={to}&textType={textType}&includeSentenceLength=true", token, body);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var translations = JsonConvert.DeserializeObject<List<TranslationReult>>(response.Content);

                return translations.FirstOrDefault()?.translations.FirstOrDefault()?.text;
            }

            throw new Exception(response.ErrorMessage);
        }

        private RestRequest GetRequest(string uri, string token, object body)
        {
            var request = new RestRequest(uri, Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", token);
            request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
            return request;
        }

        private string GetToken()
        {
            var client = new RestClient();
            var request = new RestRequest(tokenUri, Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("ocp-apim-subscription-key", translatorTokenKey);
            var response = client.Execute(request);

            return response.Content;
        }

        private class DetectResult
        {
            public string language { get; set; }
        }

        private class Translation
        {
            public string text { get; set; }
            public string  to { get; set; }
            public SentenceBoundaries sentLen { get; set; }

        }

        private class SentenceBoundaries
        {
            public List<int> srcSentLen{ get; set; }
            public List<int> transSentLen { get; set; }
        }


        private class TranslationReult
        {
            public List<Translation> translations { get; set; }
        }
    }
}
