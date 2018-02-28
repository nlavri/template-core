namespace TemplateCore.Core.Helpers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion

    public class IoHelper
    {
        private readonly AppConfiguration appConfiguration;

        public IoHelper(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
        }

        public IList<string> GetDirectories(string path)
        {
            return Flatten(path, Directory.EnumerateDirectories).ToList();
        }

        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                ForceDeleteDirectory(path);
            }
        }

        public IList<string> GetFiles(string path)
        {
            return Flatten(path, Directory.GetDirectories).SelectMany(Directory.EnumerateFiles).Where(x => !ShouldExclude(x)).ToList();
        }

        public void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            var file = new FileInfo(destinationPath);

            if (file.Directory != null && !file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.Copy(sourcePath, destinationPath);
        }

        public void RenameFile(string oldName, string newName)
        {
            var file = new FileInfo(newName);

            if (file.Directory != null && !file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.Move(oldName, newName);
        }

        private void ForceDeleteDirectory(string path)
        {
            var folders = new Stack<DirectoryInfo>();
            var root = new DirectoryInfo(path);
            folders.Push(root);

            while (folders.Count > 0)
            {
                DirectoryInfo currentFolder = folders.Pop();
                currentFolder.Attributes = currentFolder.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);

                foreach (var d in currentFolder.GetDirectories())
                {
                    folders.Push(d);
                }

                foreach (var fileInFolder in currentFolder.GetFiles())
                {
                    fileInFolder.Attributes = fileInFolder.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    fileInFolder.Delete();
                }
            }

            root.Delete(true);
        }

        private static IEnumerable<T> Flatten<T>(T item, Func<T, IEnumerable<T>> next)
        {
            yield return item;

            foreach (T flattenedChild in next(item).SelectMany(child => Flatten(child, next)))
            {
                yield return flattenedChild;
            }
        }

        private bool ShouldExclude(string path)
        {
            var segments = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            var shouldExclude =
                segments.Any(
                    directory =>
                      this.appConfiguration.GetDirectoryExclusions().Any(exclusion => Regex.IsMatch(directory, exclusion)));

            if (!shouldExclude)
            {
                var file = segments.Last();
                shouldExclude = this.appConfiguration.GetFileExclusions().Any(exclusion => Regex.IsMatch(file, exclusion));
            }

            return shouldExclude;
        }
    }
}
