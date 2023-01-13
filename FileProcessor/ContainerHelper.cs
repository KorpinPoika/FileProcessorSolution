using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using FileProcessorTool.Interfaces;

namespace FileProcessor.App
{
    public class ContainerHelper
    {
        public static ContainerHelper Init()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var assemblies = Directory.GetFiles( directory, "*.dll" ).Select(  
                AssemblyLoadContext.Default.LoadFromAssemblyPath 
            );

            var configuration = new ContainerConfiguration().WithAssemblies( assemblies );
            var container = configuration.CreateContainer();
            
            var result = new ContainerHelper();
            container.SatisfyImports(result);

            return result;
        }

        [Import]
        public ILoggerFacade Logger { get; set; }

        [Import]
        public Interactivity Interactivity { get; set; }

        [Import]
        public ActionLauncher Launcher { get; set; }
    }
}