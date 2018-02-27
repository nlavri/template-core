namespace Nlavri.Templifier.Core
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Builders;
    using Processors;

    #endregion

    public class PackageCreator
    {
        #region Fields

        private readonly ClonePackageBuilder clonePackageBuilder;
        private readonly ManifestBuilder manifestBuilder;
        private readonly TokenisedPackageBuilder packageTokeniser;
        private readonly IoHelper ioHelper;


        #endregion

        public PackageCreator(
            ClonePackageBuilder clonePackageBuilder,
            TokenisedPackageBuilder packageTokeniser,
            ManifestBuilder manifestBuilder, IoHelper ioHelper)
        {
            this.clonePackageBuilder = clonePackageBuilder;
            this.packageTokeniser = packageTokeniser;
            this.manifestBuilder = manifestBuilder;
            this.ioHelper = ioHelper;
        }

        public void CreatePackage(CommandOptions options)
        {
            var package = new Package
            {
                Path = options.Folder,
                Manifest =
                    this.manifestBuilder.Build(Path.GetFileNameWithoutExtension(options.PackagePath), options.Folder,
                        new List<string>() { options.TokenValue.Value })
            };

            var clonedPackage = this.clonePackageBuilder.Build(package);
            var tokenizedPackage = this.packageTokeniser.Tokenise(clonedPackage,
                new Dictionary<string, string>() { { options.TokenValue.Key, options.TokenValue.Value } });

            var resultFile = string.IsNullOrEmpty(Path.GetExtension(options.PackagePath))
                ? options.PackagePath + ".pkg"
                : options.PackagePath;
            ZipFile.CreateFromDirectory(tokenizedPackage.Path, resultFile, CompressionLevel.Optimal, false);

            this.ioHelper.DeleteDirectory(clonedPackage.Path);
            this.ioHelper.DeleteDirectory(tokenizedPackage.Path);
        }
    }
}