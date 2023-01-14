using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileProcessorTool.Interfaces;

namespace FileProcessor.WordsCounter
{
    [Export(typeof(IActionModule))]
    public class Module: IActionModule
    {
        private readonly ILoggerFacade _logger;
        private readonly WordCounter _wordCounter;

        [ImportingConstructor]
        public Module(ILoggerFacade logger, WordCounter wordCounter)
        {
            _logger = logger;
            _wordCounter = wordCounter;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public string Name => Info.ModuleName;
        public string Description => Info.Description;
        public string ActionTag => Info.Action;
        public string Usage => string.Join(
            Environment.NewLine, Arguments.Select(x => $@"{x.Key} // {x.Description}")    
        );

        public IEnumerable<Argument> Arguments => new Collection<Argument> {
            new Argument { Key = $"-{Keys.Input}|--input <inputFile>",    Description = "Input text file" },
            new Argument { Key = $"-{Keys.Output}|--output <outputFile>", Description = "Output file" }
        };

        public Task RunAsync(IEnumerable<Argument> arguments)
        {
            var argumentList = arguments.ToList();

            _logger.Info("WordsCounter module is started"); 
            
            
            var argsDict = ArgumentInspector.CheckArguments(Arguments, argumentList);
            
            return Task.Run(
                () => {
                    var timer = new Stopwatch();
                    timer.Start();

                    var res = _wordCounter.CountWords(argsDict[Keys.Input], Encoding.GetEncoding(1251));

                    timer.Stop();
                    
                    _logger.Info("Start to write results to file...");

                    File.WriteAllLines(
                        argsDict[Keys.Output],
                        res
                       .OrderByDescending(kvp => kvp.Value.Value)
                       .Select(kvp => $"{kvp.Key},{kvp.Value.Value}"),
                        Encoding.GetEncoding(1251)
                    );

                    _logger.Info("The counting is completed");
                    _logger.Info($"{res.Count} words are processed and saved to {argsDict[Keys.Output]}.");
                    _logger.Info($"Execution time is {timer.Elapsed}");
                }    
            );
        }

    }
}