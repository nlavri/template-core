namespace TemplateCore
{
    using System.Collections.Generic;
    using System.Linq;

    public class AppConfiguration
    {
        public string DirectoryExclusions { get; set; }

        public string FileExclusions { get; set; }

        public string TokeniseFileExclusions { get; set; }

        public IList<string> GetDirectoryExclusions()
        {
            return ParseList(DirectoryExclusions);
        }

        public IList<string> GetFileExclusions()
        {
            return ParseList(FileExclusions);
        }

        public IList<string> GetTokeniseFileExclusions()
        {
            return ParseList(TokeniseFileExclusions);
        }

        private IList<string> ParseList(string commaSeparatedString)
        {
            return string.IsNullOrEmpty(commaSeparatedString) ? new List<string>() : commaSeparatedString.Split(";".ToCharArray()).ToList();
        }
    }
}