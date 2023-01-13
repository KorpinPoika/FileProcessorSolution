using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileProcessorTool.Interfaces
{
    public interface IActionModule
    {
        string Name { get; }
        string Description { get; }

        string ActionTag { get; }

        string Usage { get; }

        IEnumerable<Argument> Arguments { get; }
            
        Task RunAsync(IEnumerable<Argument> arguments);
    }
}