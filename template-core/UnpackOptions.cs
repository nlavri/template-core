namespace TemplateCore
{
    using System;
    using System.Collections.Generic;
    using CommandLine;
    using CommandLine.Text;

    [Verb("unpack", HelpText = "Unpacks a template package into a folder")]
    class UnpackOptions : CommandOptions
    {
        private IEnumerable<string> rawTokens;

        public UnpackOptions()
        {
            this.TokensPairs = new Dictionary<string, string>();
        }

        [Option('f', "folder", Required = true, HelpText = "Destination folder.")]
        public string Folder { get; set; }

        [Option('p', "package", Required = true, HelpText = "Source package to be deployed.")]
        public string PackagePath { get; set; }

        [Option('t', "tokens", Separator = ';', Required = true, HelpText = "Semicolon (;) delimited regex=string pairs for replacement.")]
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
                    var token = split[0];

                    if (string.IsNullOrEmpty(token))
                    {
                        throw new ArgumentException("Empty token provided for replacement");
                    }

                    if (split.Length == 2)
                    {
                        this.TokensPairs.Add(token, split[1] ?? string.Empty);
                    }
                    else
                    {
                        this.TokensPairs.Add(indexFallback, token);
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
                yield return new Example("Unpack scenario",
                    new PackOptions()
                    {
                        Folder = "C:\\Projects\\MySolutionFolder",
                        PackagePath = "C:\\ProjectsTemplates\\template.pkg",
                        Tokens = new[] { "NewNamespace" }
                    });
                yield return new Example("Pack scenario with custom token",
                    new PackOptions()
                    {
                        Folder = "C:\\Projects\\MySolutionFolder",
                        PackagePath = "C:\\ProjectsTemplates\\template.pkg",
                        Tokens = new[] { "__ROOT_TOKEN_=NewNamespace;__API_TOKEN__=NewController" }
                    });
            }
        }
    }
}
