using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileProcessor.WordsCounter.UnitTests
{
    [TestClass]
    public class WordCounterTests
    {
        [TestMethod]
        public void EmptySourceTest()
        {
            var mock = new Mock<ILineReader>();
            mock.Setup(lr => lr.ReadLines(string.Empty, Encoding.GetEncoding(0)))
                .Returns(() => new List<string>());

            var res = new WordCounter(mock.Object).CountWords(string.Empty, Encoding.GetEncoding(0));

            Assert.IsFalse(res.Any(), "For empty lines we expect empty results");
        }

        [TestMethod]
        public void CountWordsTest()
        {
            var mock = new Mock<ILineReader>();
            mock.Setup(lr => lr.ReadLines(string.Empty, Encoding.UTF8))
                .Returns(
                    () => new List<string> {
                        "one, two, three",
                        "one, two. three",
                        "one  three"
                    }
                 );

            var res = new WordCounter(mock.Object).CountWords(string.Empty, Encoding.UTF8);

            Assert.IsTrue(res["one"].Value == 3, "We expect to meet word [one] 3 times");
            Assert.IsTrue(res["two"].Value == 2, "We expect to meet word [two] 2 times");
        }
    }
}