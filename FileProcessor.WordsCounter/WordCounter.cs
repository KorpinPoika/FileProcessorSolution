using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.WordsCounter
{
    [Export]
    public class WordCounter
    {
        private readonly ILineReader _lineReader;

        [ImportingConstructor]
        public WordCounter(ILineReader lineReader)
        {
            _lineReader = lineReader;
        }

        public IDictionary<string, Counter> CountWords(string inputFile, Encoding encoding)
        {
            var result = new ConcurrentDictionary<string, Counter>();

            Parallel.ForEach(
                _lineReader.ReadLines(inputFile, encoding),
                line => {
                    foreach (var word in line.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.GetOrAdd(word.ClearExtraSymbols(), _ => new Counter()).Inc();
                    }
                }
            );

            return result;
        }

        
    }
}