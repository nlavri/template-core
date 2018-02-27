namespace Nlavri.Templifier
{
    using System.IO;
    using Core;
    using Core.Builders;
    using Core.Processors;
    using Core.Tokeniser;
    using Microsoft.Extensions.Configuration;
    using SimpleInjector;

    public static class IoC
    {
        public static Container Init()
        {
            var container = new Container();
            container.Options.AllowOverridingRegistrations = true;

            container.RegisterSingleton<PackageCreator>();
            container.RegisterSingleton<PackageDeployer>();
            container.RegisterSingleton<TemplateTokeniser>();
            container.RegisterSingleton<ManifestBuilder>();
            container.RegisterSingleton<TokenisedPackageBuilder>();
            container.RegisterSingleton<ClonePackageBuilder>();
            container.RegisterSingleton<FileContentProcessor>();
            container.RegisterSingleton<IoHelper>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var appConfig = new AppConfiguration()
            {
                DirectoryExclusions = configuration["AppSettings:DirectoryExclusions"],
                FileExclusions = configuration["AppSettings:FileExclusions"],
                TokeniseFileExclusions = configuration["AppSettings:TokeniseFileExclusions"],
            };

            container.RegisterSingleton(appConfig);

            return container;
        }
    }
}
