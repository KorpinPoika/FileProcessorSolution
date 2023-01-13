using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Text;

namespace FileProcessor.WordsCounter
{
    [Export(typeof(ILineReader))]
    public class LineReader: ILineReader
    {
        public IEnumerable<string> ReadLines(string path, Encoding encoding) => File.ReadLines(path, encoding);
    }
}