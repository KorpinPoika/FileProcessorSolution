using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;
using FileProcessorTool.Interfaces;

namespace FileProcessor.App
{
    [Export]
    public class Interactivity
    {
        private readonly ILoggerFacade _logger;

        [ImportingConstructor]
        public Interactivity(ILoggerFacade logger)
        {
            _logger = logger;
        }

        [ImportMany]
        public IEnumerable<IActionModule> ActionModules { get; set; }


        public void Greeting()
        {
            var assemblyVersion = this.GetType().Assembly.GetName().Version.ToString();
            _logger.Info($"Welcome to file`s processor utility. Version: {assemblyVersion}");            
        }

        public void PrintUsage()
        {
            var actionModules = ActionModules.ToList();

            _logger.Info("The usage is:");
            _logger.Info("FileProcessorTool -a <action> [action`s parameters]");
            _logger.Info($"{Environment.NewLine}{actionModules.Count} module(s) found:");
            
            actionModules.ForEach(
                m => {
                    _logger.Info($"---- Module: {m.Name} -----");
                    _logger.Info($"    description : {m.Description}");
                    _logger.Info($"    action : {m.ActionTag}");
                    _logger.Info($"    arguments: {m.Usage}");
                    _logger.Info($"--------");
                }    
            );
        }
    }
}