namespace Nlavri.Templifier
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using CommandLine;
    using CommandLine.Text;

    #endregion

    public class CommandOptions
    {
        private string rawMode;
        private string rawTokens;

        public CommandOptions()
        {
            this.SelectedMode = Mode.NotSet;
            this.TokenValue = new KeyValuePair<string, string>();
        }

        public Mode SelectedMode { get; private set; }

        [Option('f', "folder", Required = true, HelpText = "Source path to be used when creating a package, or the destination path when deploying.")]
        public string Folder { get; set; }

        [Option('p', "package", Required = true, HelpText = @"Package to be created, or package to be deployed.")]
        public string PackagePath { get; set; }

        [Option('m', "mode", Required = true, HelpText = "Specifies whether to (c)reate/(d)eploy a package")]
        public string RawMode
        {
            get
            {
                return this.rawMode;
            }

            set
            {
                this.rawMode = value;

                switch (this.rawMode.ToLowerInvariant())
                {
                    case "c":
                        this.SelectedMode = Mode.Create;
                        break;
                    case "d":
                        this.SelectedMode = Mode.Deploy;
                        break;
                }
            }
        }

        [Option('t', "token", Required = true, HelpText = "Specifies name=value to be used as token replacement.")]
        public string Token
        {
            get => this.rawTokens;

            set
            {
                this.rawTokens = value;

                var tokens = this.rawTokens.Split('=');


                if (tokens.Length != 2 || string.IsNullOrEmpty(tokens[0]) || string.IsNullOrEmpty(tokens[1]))
                {
                    throw new ArgumentException("Token is malformed");
                }
                this.TokenValue = new KeyValuePair<string, string>(tokens[0], tokens[1]);

            }
        }

        public KeyValuePair<string, string> TokenValue { get; private set; }

        //[Option('h', HelpText = "Display this help text.")]
        //public string GetUsage()
        //{
        //    var help = new HelpText(new HeadingInfo("Templify",
        //        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()))
        //    {
        //        AdditionalNewLineAfterOption = false,
        //        MaximumDisplayWidth = Console.WindowWidth,
        //        Copyright = new CopyrightInfo("nlavri", 2016)
        //    };

        //    help.AddPreOptionsLine("Usage:");
        //    help.AddPreOptionsLine("    TemplifyCmd.exe -m c -f C:\\MySolution -p C:\\MyTemplate.pkg -t \"MySolution=__NAME__\"");
        //    help.AddPreOptionsLine("    TemplifyCmd.exe -m d -f C:\\MySolution -p C:\\MyTemplate.pkg -t \"__NAME__=MyNewSolution\" ");

        //    help.AddOptions(this);

        //    return help;
        //}

        public enum Mode
        {
            NotSet,
            Create,
            Deploy,
        }
    }
}