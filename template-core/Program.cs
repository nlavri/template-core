namespace TemplateCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using CommandLine;
    using CommandLine.Text;
    using Core;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return new Parser(config => config.HelpWriter = Console.Out)
                    .ParseArguments<PackOptions, UnpackOptions>(args)
                    .MapResult(
                        (PackOptions opts) => RunPackAndReturnExitCode(opts),
                        (UnpackOptions opts) => RunUnpackAndReturnExitCode(opts),
                        errs => 1);
            }
            catch (TargetInvocationException invocationException)
            {
                LogException(invocationException.InnerException ?? invocationException);
            }
            catch (Exception exception)
            {
                LogException(exception);
            }

            return 0;
        }

        private static void LogException(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Error.WriteLine("An error encountered : ");
            Console.Error.WriteLine(exception.Message);
            Console.ResetColor();
        }

        private static int RunUnpackAndReturnExitCode(UnpackOptions options)
        {
            var container = IoC.Init();
            container.GetInstance<UnpackCommand>().Unpack(options);

            return 0;
        }

        private static int RunPackAndReturnExitCode(PackOptions options)
        {
            var container = IoC.Init();

            container.GetInstance<PackCommand>().CreatePackage(options);

            return 0;
        }

    }
}
