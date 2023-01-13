using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Akka.Actor;

namespace FileProcessor.WordsCounter.Akkas
{
    public class WordsCounterActor: ReceiveActor
    {
        private readonly List<string>_allWords = new List<string>();

        public sealed class WriteResultMessage
        {
            public string OutputFilePath { get; set; }
        }

        public WordsCounterActor()
        {
            Receive<IEnumerable<string>>(
                words => _allWords.AddRange(words)    
            );

            Receive<WriteResultMessage>(msg => WriteResults(msg.OutputFilePath));
        }

        private void WriteResults(string outputFilePath)
        {
            var result = _allWords.GroupBy(
                x => x, (x,g) => new {word = x, cnt = g.Count()}
            )
           .OrderByDescending(x => x.cnt)
           .ToList();

            File.WriteAllLines(
                outputFilePath, 
                result.Select(x => $"{x.word},{x.cnt}"),
                Encoding.GetEncoding(1251)
            );

            Sender.Tell(result.Count);
        }
    }
}