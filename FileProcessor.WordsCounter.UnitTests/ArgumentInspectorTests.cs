using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileProcessorTool.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileProcessor.WordsCounter.UnitTests
{
    [TestClass]
    public class ArgumentInspectorTests
    {
        private static readonly ICollection<Argument> expected = new Collection<Argument> {
            new Argument {Key = $"-{Keys.Input}|--input:<inputFile>", Description = "Input text file"},
            new Argument {Key = $"-{Keys.Output}|--output:<outputFile>", Description = "Output file"}
        };

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongArgsTest()
        {
            ArgumentInspector.CheckArguments(
               expected,
               new List<Argument> {
                   new Argument { Key = "a"}
               } 
            );
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void WrongInputFileArgsTest()
        {
            ArgumentInspector.CheckArguments(
               expected,
               new List<Argument> {
                   new Argument { Key = "i", Value = @"/home/alexey/nonexistent.txt"},
                   new Argument { Key = "o", Value = @"/home/alexey/result.txt"},
               } 
            );
        }

        [TestMethod]
        public void ArgsTest()
        {
            ArgumentInspector.CheckArguments(
               expected,
               new List<Argument> {
                   new Argument { Key = "i", Value = @"../../../../TestData/testTask.txt"},
                   new Argument { Key = "o", Value = @"/home/alexey/result.txt"},
               } 
            );
        }
    }
}