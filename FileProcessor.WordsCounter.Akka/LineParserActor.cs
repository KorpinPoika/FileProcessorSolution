using System;
using System.Linq;
using System.Text.RegularExpressions;
using Akka.Actor;

namespace FileProcessor.WordsCounter.Akkas
{
    public class LineParserActor: ReceiveActor
    {
        private readonly IActorRef _counterRef;

        public static Props Props(IActorRef counterRef) => Akka.Actor.Props.Create(
            () => new LineParserActor(counterRef)
        );

        public LineParserActor(IActorRef counterRef)
        {
            _counterRef = counterRef;
            Receive<string>(line => ProsessLine(line));
        }

        private void ProsessLine(string line)
        {
            var words = line.Trim().Split(
                " ", StringSplitOptions.RemoveEmptyEntries
            )
           .Select(ClearExtraSymbols)
           .ToList();

            _counterRef.Tell(words);
        }

        private static string ClearExtraSymbols(string word) => Regex.Replace(word, "[\\?\\!@,\\.\";'\\\\]", "");
    }
}