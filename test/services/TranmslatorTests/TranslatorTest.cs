using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TranslatorIntegration.Services.Implementation;
using TrnaslatorIntegration.Services.Interfaces;

namespace TranmslatorTests
{
    [TestClass]
    public class TranslatorTest
    {
        private string translatorKey ="";
        private string translatorTokenUri = "";
        private string translatorUri = "";

        [TestMethod]
        public async Task Detectlanguage()
        {
            ITranslator translator = new Translator(translatorTokenUri, translatorKey, translatorUri);

           var result = await translator.DetectLanguage("Hallo ich bin Michael!");
            result.Should().Be("de");
        }

        [TestMethod]
        public async Task TranslateSimpleText()
        {
            ITranslator translator = new Translator(translatorTokenUri, translatorKey, translatorUri);

            var result = await translator.Translate("Hallo ich bin Michael.", "de", "en");
            result.Should().Be("Hi I'm Michael.");
        }

        [TestMethod]
        public async Task TranslateHTMLText()
        {
            ITranslator translator = new Translator(translatorTokenUri, translatorKey, translatorUri);
            var file = "./files/imageExtractionTest.html";
            var fc = File.ReadAllText(file);
            var result = await translator.Translate(fc, "de", "en", "html");
            result.Should().NotBe(fc);
        }

        [TestMethod]
        public async Task TranslateLongHTMLText()
        {
            ITranslator translator = new Translator(translatorTokenUri, translatorKey, translatorUri);
            var file = "./files/long.html";
            var fc = File.ReadAllText(file);
            var result = await translator.Translate(fc, "es", "en", "html");
            result.Should().NotBe(fc);
        }
    }
}