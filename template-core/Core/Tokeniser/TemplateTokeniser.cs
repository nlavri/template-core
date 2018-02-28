namespace TemplateCore.Core.Tokeniser
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Helpers;

    #endregion

    public class TemplateTokeniser
    {
        private readonly FileContentHelper fileContentProcessor;
        private readonly IoHelper ioHelper;

        public TemplateTokeniser(FileContentHelper fileContentProcessor, IoHelper ioHelper)
        {
            this.fileContentProcessor = fileContentProcessor;
            this.ioHelper = ioHelper;
        }

        public void TokeniseDirectoryAndFilePaths(string file, Dictionary<string, string> tokens)
        {
            var tokenisedName = Replace(tokens, file);
            this.ioHelper.RenameFile(file, tokenisedName);
        }

        public void TokeniseFileContent(string file, Dictionary<string, string> tokens)
        {
            var contents = this.fileContentProcessor.ReadContents(file);
            contents = Replace(tokens, contents);
            this.fileContentProcessor.WriteContents(file, contents);
        }

        private static string Replace(Dictionary<string, string> tokens, string value)
        {
            return tokens.Aggregate(value, (current, token) => Regex.Replace(current, token.Key, match => token.Value));
        }
    }
}