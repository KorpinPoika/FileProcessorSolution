using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using FileProcessorTool.Interfaces;
using Microsoft.Extensions.CommandLineUtils;

namespace FileProcessor.App
{
    [Export]
    public class ActionLauncher
    {
        [Import]
        public ILoggerFacade Logger { get; set; }

        [ImportMany]
        public ICollection<IActionModule> ActionModules { get; set; }

        public Task LaunchAsync(IEnumerable<string> args)
        {
            var app = new CommandLineApplication { Name = "FileProcessor"};
            app.HelpOption("-?|-h|--help");
            
            var actOption = app.Option("-a|--action <actionTag>", "Action type", CommandOptionType.SingleValue);

            RegisterModulesArguments(app);

            app.Execute( args.ToArray() );

            if (actOption.HasValue())
            {
                var action = ActionModules.FirstOrDefault(a => a.ActionTag == actOption.Value());
                if (action == null) {
                    throw new ArgumentOutOfRangeException(
                        "action",
                        $"Cannot find action for key={actOption.Value()}"    
                    );
                }

                return action.RunAsync(
                    app
                   .Options
                   .Select(
                       x => new Argument {
                           Key = x.ShortName, 
                           Value = x.Value()
                       }
                    )
                   .ToList()    
                );
            }
            
            app.ShowHint();
            throw new ArgumentException("No actions detected. Please use -h option for help");
        }

        private void RegisterModulesArguments(CommandLineApplication app)
        {
            ActionModules
           .SelectMany(a => a.Arguments)
           .GroupBy(x => x.Key)
           .Select(g => g.FirstOrDefault())
           .ToList()
           .ForEach(
                a => app.Option(a.Key, a.Description, CommandOptionType.SingleValue)
            );
        }
    }
}