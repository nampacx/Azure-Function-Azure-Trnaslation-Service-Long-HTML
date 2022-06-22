using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TranslatorIntegration.Helpers;

namespace LengthCheckerTests
{
    [TestClass]
    public class HTMLLengthCheck
    {
        [TestMethod]
        public void CloseBiggerThanIndex()
        {
            var index = 1211;

            var file = "./files/Sample2.html";
            var fc = File.ReadAllText(file);

            var close = LengthCheck.CloseToOpen("<", index, fc);

            Assert.IsTrue(close > index);
        }

        [TestMethod]
        public void CloseSmallerThanIndex()
        {
            var index = 1242;

            var file = "./files/Sample2.html";
            var fc = File.ReadAllText(file);

            var close = LengthCheck.CloseToOpen("<", index, fc);

            Assert.IsTrue(close <= index);
        }

        [TestMethod]
        public void CutIndexSmaller()
        {
            var index = 1242;

            var file = "./files/Sample2.html";
            var fc = File.ReadAllText(file);

            var close = LengthCheck.CuttingIndex(fc, index);

            Assert.IsTrue(close <= index);
        }

        [TestMethod]
        public void GetChunk()
        {
            var index = 180;

            var file = "./files/HTMLTranslationSample.html";
            var fc = File.ReadAllText(file);

            var chunks = LengthCheck.GetChunks(fc, index).ToList();

            Assert.IsTrue(chunks.Count > 1);
        }

        [TestMethod]
        public void GetChunkLong()
        {
            var index = 5000;

            var file = "./files/long.html";
            var fc = File.ReadAllText(file);

            var chunks = LengthCheck.GetChunks(fc, index).ToList();

            Assert.IsTrue(chunks.Count > 1);
        }


        [TestMethod]
        public void GetChunkXLong()

        {
            var index = 5000;
            var file = "./files/long.html";
            var fc = File.ReadAllText(file);
            for (int i = 0; i < 8; i++)
            {
                fc += fc;

                var chunks = LengthCheck.GetChunks(fc, index).ToList();
                Debug.WriteLine(fc.Length);
                Debug.WriteLine(chunks.Count);
                Assert.IsTrue(chunks.Count > 1);
            }

        }

        [TestMethod]
        public void SentenceStart()
        {
            var index = 1376;

            var file = "./files/Sample2.html";
            var fc = File.ReadAllText(file);

            var start = LengthCheck.SentenceStart(fc, index);

            Assert.IsTrue(start <= index);
        }
    }
}
