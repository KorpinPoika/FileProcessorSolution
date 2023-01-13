namespace FileProcessorTool.Interfaces
{
    public interface ILoggerFacade
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}