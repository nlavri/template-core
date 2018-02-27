using System;

namespace Nlavri.Templifier
{
    using System.Collections.Generic;
    using System.IO;
    using CommandLine;
    using Core;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new Parser(config => config.HelpWriter = Console.Out).ParseArguments<CommandOptions>(args)
                    .WithParsed(RunOptionsAndReturnExitCode)
                    .WithNotParsed(HandleParseError); ;
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error encountered : ");
                Console.WriteLine(exception.Message);
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {

        }

        private static void RunOptionsAndReturnExitCode(CommandOptions options)
        {
            var container = IoC.Init();

            switch (options.SelectedMode)
            {
                case CommandOptions.Mode.Create:
                    container.GetInstance<PackageCreator>().CreatePackage(options);
                    break;
                case CommandOptions.Mode.Deploy:
                    container.GetInstance<PackageDeployer>().DeployPackage(options);
                    break;
            }
        }
    }
}
