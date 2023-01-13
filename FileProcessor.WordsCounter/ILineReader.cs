using System.Collections.Generic;
using System.Text;

namespace FileProcessor.WordsCounter
{
    public interface ILineReader
    {
        IEnumerable<string> ReadLines(string path, Encoding encoding);
    }
}