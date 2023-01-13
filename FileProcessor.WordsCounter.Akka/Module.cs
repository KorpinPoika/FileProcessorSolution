using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using Akka.Streams;
using Akka.Streams.Dsl;
using FileProcessorTool.Interfaces;

namespace FileProcessor.WordsCounter.Akkas
{
    [Export(typeof(IActionModule))]
    public class Module: IActionModule
    {
        private readonly ILoggerFacade _logger;

        [ImportingConstructor]
        public Module(ILoggerFacade logger)
        {
            _logger = logger;
        }

        public string Name => "WordsCounter.Akka.Net";
        public string Description => "Words counter based on Akka.Net";
        public string ActionTag => "wakka";
        public string Usage => string.Join(
            Environment.NewLine, Arguments.Select(x => $@"{x.Key} // {x.Description}")    
        );

        public IEnumerable<Argument> Arguments => new Collection<Argument> {
            new Argument { Key = $"-i|--input:<inputFile>",    Description = "Input text file" },
            new Argument { Key = $"-o|--output:<outputFile>", Description = "Output file" }
        };

        public async Task RunAsync(IEnumerable<Argument> arguments)
        {
            _logger.Info($"{Name} module is started");

            var argumentList = arguments.ToList();

            using (var system = ActorSystem.Create("system"))
            using (var materializer = system.Materializer())
            {
                var props = LinesPublisher.Props( argumentList.Single(x => x.Key.StartsWith("i")).Value );
                var source = Source.ActorPublisher<string>(props);

                var wcActor = system.ActorOf<WordsCounterActor>();

                //var actorProps = Props.Create<LineParserActor>().WithRouter(
                var actorProps = LineParserActor.Props(wcActor).WithRouter(
                    new RoundRobinPool(Environment.ProcessorCount*5)
                );
                var parser = system.ActorOf(actorProps,"parser-actor");

                await source.RunForeach(
                    line => parser.Forward(line),
                    materializer
                );

                var allWords = await wcActor.Ask<int>(
                    new WordsCounterActor.WriteResultMessage {
                        OutputFilePath = argumentList.Single(x => x.Key.StartsWith("o")).Value
                    }
                );

                _logger.Info($" ## allWords:{allWords} ##");
            }

            _logger.Info("~~~~~~~` akka has done ~~~~~");
        }
    }
}