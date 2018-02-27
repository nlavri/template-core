namespace Nlavri.Templifier.Core
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Processors;
    using Tokeniser;

    #endregion

    public class PackageDeployer
    {
        #region Fields

        private readonly TemplateTokeniser templateTokeniser;

        private readonly AppConfiguration appConfiguration;

        private readonly IoHelper ioHelper;

        #endregion

        public PackageDeployer(TemplateTokeniser templateTokeniser, AppConfiguration appConfiguration, IoHelper ioHelper)
        {
            this.templateTokeniser = templateTokeniser;
            this.appConfiguration = appConfiguration;
            this.ioHelper = ioHelper;
        }

        public void DeployPackage(CommandOptions options)
        {
            ZipFile.ExtractToDirectory(options.PackagePath, options.Folder);

            var tokens = new Dictionary<string, string>() { { options.TokenValue.Key, options.TokenValue.Value } };

            ProcessFiles(options.Folder, tokens);
            CleanUp(options.Folder, tokens);
        }

        private void CleanUp(string path, Dictionary<string, string> tokens)
        {
            var directories = this.ioHelper.GetDirectories(path).ToList();

            foreach (var directory in directories)
            {
                foreach (var token in tokens)
                {
                    if (directory.Contains(token.Key))
                    {
                        this.ioHelper.DeleteDirectory(directory);
                    }
                }
            }
        }

        private void ProcessFiles(string path, Dictionary<string, string> tokens)
        {
            var files = this.ioHelper.GetFiles(path);

            ProcessFileContents(files, tokens);
            ProcessDirectoryAndFilePaths(files, tokens);
        }

        private void ProcessDirectoryAndFilePaths(IEnumerable<string> files, Dictionary<string, string> tokens)
        {
            foreach (var file in files)
            {
                this.templateTokeniser.TokeniseDirectoryAndFilePaths(file, tokens);
            }
        }

        private void ProcessFileContents(IEnumerable<string> files, Dictionary<string, string> tokens)
        {
            var filteredFiles =
                files.Where(
                    file =>
                        !this.appConfiguration.GetTokeniseFileExclusions()
                            .Contains((Path.GetExtension(file) ?? string.Empty).ToLowerInvariant())).ToList();

            foreach (var file in filteredFiles)
            {
                this.templateTokeniser.TokeniseFileContent(file, tokens);
            }
        }
    }
}