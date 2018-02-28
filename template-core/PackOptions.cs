using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateCore
{
    using System.Linq;
    using CommandLine;
    using CommandLine.Text;

    [Verb("pack", HelpText = "Creates a template package from a source folder")]
    class PackOptions : CommandOptions
    {
        private IEnumerable<string> rawTokens;

        public PackOptions()
        {
            this.TokensPairs = new Dictionary<string, string>();
        }

        [Option('f', "folder", Required = true, HelpText = "Source folder to be used when creating a package.")]
        public string Folder { get; set; }

        [Option('p', "package", Required = true, HelpText = @"Template package path to be created.")]
        public string PackagePath { get; set; }

        [Option('t', "tokens", Separator = ';', Required = true, HelpText = "Semicolon (;) delimited regex=string pairs for replacement. Regex is required but replacement is optional, zero bazed __TOKEN[i]__ used in this case.")]
        public IEnumerable<string> Tokens
        {
            get => this.rawTokens;

            set
            {

                this.rawTokens = value;
                this.TokensPairs.Clear();

                int i = 0;
                foreach (var pair in value)
                {
                    var split = pair.Split('=');
                    var indexFallback = $"__TOKEN{i}__";
                    var key = split[0];

                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentException("Empty regex provided for replacement");
                    }

                    if (split.Length == 2)
                    {
                        this.TokensPairs.Add(key, string.IsNullOrEmpty(split[1]) ? indexFallback : split[1]);
                    }
                    else
                    {
                        this.TokensPairs.Add(key, indexFallback);
                    }

                    i++;
                }
            }
        }

        public Dictionary<string, string> TokensPairs { get; }

        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Pack scenario",
                    new PackOptions()
                    {
                        Folder = "C:\\Projects\\MySolutionFolder",
                        PackagePath = "C:\\ProjectsTemplates\\template.pkg",
                        Tokens = new[] { "MyRootNamespace" }
                    });
                yield return new Example("Pack scenario with custom token",
                    new PackOptions()
                    {
                        Folder = "C:\\Projects\\MySolutionFolder",
                        PackagePath = "C:\\ProjectsTemplates\\template.pkg",
                        Tokens = new[] { "MyRootNamespace=__ROOT_TOKEN_;ApiController=__API_TOKEN__" }
                    });
            }
        }
    }
}
