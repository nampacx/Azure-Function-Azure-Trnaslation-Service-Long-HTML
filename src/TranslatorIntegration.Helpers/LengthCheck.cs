namespace TranslatorIntegration.Helpers
{
    public static class LengthCheck
    {
        readonly static Dictionary<string, string> openClose = new Dictionary<string, string>()
        {
            { "<", ">"}
        };

        readonly static List<string> SentenceEnd = new List<string>() { "." };

        public static int SentenceStart(string input, int maxSize)
        {
            var index = -1;

            foreach (var end in SentenceEnd)
            {
                index = input.LastIndexOf(end, maxSize);
            }

            return index + 1;
        }

        /// <summary>
        /// Splits the input text in chunks < maxSize. 
        /// Also checks if the splitting index is within a html tag. 
        /// If this happens the methods looks for the start of the tag and only moves the splitting index before the start tag.
        /// </summary>
        /// <param name="input">Text to junk</param>
        /// <param name="maxSize">Max chunk size</param>
        /// <returns>List of chunks smaller than maxSize</returns>
        public static IEnumerable<string> GetChunks(string input, int maxSize)
        {
            if (input.Length <= maxSize)
            {
                yield return input;
            }
            else
            {
                var toCut = input;
                do
                {
                    var cutting = CuttingIndex(toCut, maxSize);
                    var p1 = toCut.Substring(0, cutting);

                    yield return p1;

                    toCut = toCut.Substring(cutting);
                }
                while (toCut.Length > maxSize);

                yield return toCut;
            }
        }

        public static int CuttingIndex(string input, int maxSize)
        {
            var closeToOpen = CloseToOpenTuple("<", maxSize, input);

            if (maxSize < closeToOpen.Item2)
            {
                return closeToOpen.Item1;
            }
            else
            {
                return maxSize;
            }
        }

        public static int CloseToOpen(string open, int index, string input)
        {
            var cTOT = CloseToOpenTuple(open, index, input);

            return cTOT.Item2;
        }

        private static Tuple<int, int> CloseToOpenTuple(string open, int index, string input)
        {
            var openIndex = input.LastIndexOf(open, index);

            return new Tuple<int, int>(openIndex, input.IndexOf(openClose[open], openIndex));
        }
    }
}
