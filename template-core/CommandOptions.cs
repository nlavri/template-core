namespace TemplateCore
{
    abstract class CommandOptions
    {
        protected const string TokenPlaceholderValue = "__NAME__";
       
        protected CommandOptions()
        {
        }

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

    }
}