using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileProcessorTool.Interfaces;

namespace FileProcessor.WordsCounter
{
    public static class ArgumentInspector
    {
        public static IDictionary<string, string> CheckArguments(IEnumerable<Argument> expected, IEnumerable<Argument> passed)
        {
            var args = expected.GroupJoin(
                passed,
                x => x.GetKey(),
                y => y.Key,
                (x, g) => new {key = x.GetKey(), arg = x, value = g.Select(_ => _.Value).FirstOrDefault()}
            )                
           .ToList();

            var emptyArgs = args.Where(x => string.IsNullOrWhiteSpace(x.value)).ToList();

            if (emptyArgs.Any()) {
                throw new ArgumentException(
                    $"Arguments expected: {string.Join(",", emptyArgs.Select(x => x.arg.Key))}"    
                );
            }

            var argDict = args.ToDictionary(x => x.key, x => x.value);

            CheckInputFile(argDict[Keys.Input]);

            return argDict;
        }

        private static void CheckInputFile(string inputFile)
        {
            if (!File.Exists(inputFile)) {
                throw new FileNotFoundException("Input file is not found", inputFile);
            }

            //todo: what else?
        }
    }
}