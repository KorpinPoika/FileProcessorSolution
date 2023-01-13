using System;
using System.Linq;
using System.Text.RegularExpressions;
using FileProcessorTool.Interfaces;

namespace FileProcessor.WordsCounter
{
    public static class Extensions
    {
        public static string GetKey(this Argument arg)
        {
            //todo: parse with regExp?
            return arg
                  .Key
                  .Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries)
                  .First()
                  .Substring(1);
        }

        public static string ClearExtraSymbols(this string word) => Regex.Replace(word, "[\\?\\!@,\\.\";'\\\\]", "");
    }
}