using System.Threading;

namespace FileProcessor.WordsCounter
{
    public class Counter
    {
        private int _value = 0;

        public int Value => _value;

        public void Inc()
        {
            Interlocked.Increment(ref _value);
        }
    }
}