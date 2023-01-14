using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace FileProcessor.App
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var container = ContainerHelper.Init();

                container.Interactivity.Greeting();
                if (!args.Any()) {
                    container.Interactivity.PrintUsage();
                    return;
                }

                try
                {
                    await container.Launcher
                        .LaunchAsync(args)
                        .ConfigureAwait(false);

                    container.Logger.Info("Done.");
                }
                catch (CommandParsingException ex)
                {
                    container.Logger.Error(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    container.Logger.Error(ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    container.Logger.Error(ex.Message);
                }
                catch (Exception ex)
                {
                    container.Logger.Error("Fatal error:");
                    container.Logger.Error(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can`t start the program:");
                Console.WriteLine(ex);
            }

            NLog.LogManager.Shutdown();
        }
    }
}