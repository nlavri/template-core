namespace TemplateCore.Core.Builders
{
    #region Using Directives

    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading.Tasks;
    using Helpers;
    using Newtonsoft.Json;

    #endregion

    public class ClonePackageBuilder
    {
        private readonly ManifestBuilder manifestBuilder;

        private readonly IoHelper ioHelper;

        public ClonePackageBuilder(ManifestBuilder manifestBuilder, IoHelper ioHelper)
        {
            this.manifestBuilder = manifestBuilder;
            this.ioHelper = ioHelper;
        }

        public Package Build(Package package)
        {
            var newPath = Path.Combine(Path.Combine(Path.GetTempPath(), package.Manifest.Id.ToString()), "Cloned");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var result = new Package
            {
                Manifest = this.manifestBuilder.Build(package.Manifest.Name, newPath, package.Manifest.Tokens),
                Path = newPath
            };

            var files = new BlockingCollection<string>();

            Parallel.ForEach(
                package.Manifest.Files,
                file =>
                    {
                        var clonedPath = Path.Combine(result.Path, file);

                        this.ioHelper.CopyFile(Path.Combine(package.Path, file), clonedPath);

                        files.Add(clonedPath);
                    });
            foreach (var manifestFile in files)
            {
                result.Manifest.Files.Add(manifestFile);
            }

            var manifestFilePath = this.PersistManifestFileAndReturnLocation(result);
            // Add the manifest file so that it will be tokenised.
            result.Manifest.Files.Add(manifestFilePath);

            return result;
        }

        private string PersistManifestFileAndReturnLocation(Package package)
        {
            var manifestFilePath = Path.Combine(package.Path, "manifest.json");

            using (var file = File.CreateText(manifestFilePath))
            {
                file.Write(JsonConvert.SerializeObject(package));
            }

            return manifestFilePath;
        }
    }
}